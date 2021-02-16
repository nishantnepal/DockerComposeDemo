terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = ">= 2.46.0"
    }
  }
}

provider "azurerm" {
  features {}
}

resource "azurerm_kubernetes_cluster" "demo" {
    name                = "${var.aks_cluster_name}"
    location            = "${azurerm_resource_group.aks.location}"
    dns_prefix          = "ps-${var.aks_cluster_name}"
    resource_group_name = "${azurerm_resource_group.aks.name}"

    linux_profile {
        admin_username = "nodeadmin"

        ssh_key {
            key_data = "${file(var.public_ssh_key_path)}"
        }
    }

    agent_pool_profile {
        name            = "nodepool01"
        count           = "3"
        vm_size         = "Standard_DS2_v2"
        os_type         = "Linux"
        os_disk_size_gb = 30

        # Required for advanced networking
        vnet_subnet_id = "${azurerm_subnet.cluster.id}"
    }

    service_principal {
        client_id     = "${azuread_service_principal.sp-aks.application_id}"
        client_secret = "${random_string.sp-aks-password.result}"
    }

    network_profile {
        network_plugin = "azure"
    }

    addon_profile {
        oms_agent {
            enabled = true
            log_analytics_workspace_id = "${azurerm_log_analytics_workspace.aks.id}"
        }
    }
}

resource "random_id" "log_analytics_workspace_name_suffix" {
    byte_length = 8
}

resource "azurerm_log_analytics_workspace" "aks" {
    name                = "${var.log_analytics_workspace_name}-${random_id.log_analytics_workspace_name_suffix.dec}"
    location            = "${var.log_analytics_workspace_location}"
    resource_group_name = "${azurerm_resource_group.aks.name}"
    sku                 = "${var.log_analytics_workspace_sku}"

    lifecycle {
        ignore_changes = [
            name,
        ]
  }
}


resource "azurerm_log_analytics_solution" "aks-containerinsights" {
    solution_name         = "ContainerInsights"
    location              = "${azurerm_log_analytics_workspace.aks.location}"
    resource_group_name   = "${azurerm_resource_group.aks.name}"
    workspace_resource_id = "${azurerm_log_analytics_workspace.aks.id}"
    workspace_name        = "${azurerm_log_analytics_workspace.aks.name}"

    plan {
        publisher = "Microsoft"
        product   = "OMSGallery/ContainerInsights"
    }
}


resource "azurerm_virtual_network" "demo" {
  name                = "${azurerm_resource_group.aks.name}-network"
  location            = "${azurerm_resource_group.aks.location}"
  resource_group_name = "${azurerm_resource_group.aks.name}"
  address_space       = ["10.1.0.0/16"]
}

resource "azurerm_subnet" "cluster" {
  name                  = "cluster-01"
  resource_group_name   = "${azurerm_resource_group.aks.name}"
  address_prefix        = "10.1.0.0/24"
  virtual_network_name  = "${azurerm_virtual_network.demo.name}"
  route_table_id        = "${azurerm_route_table.cluster-01.id}"
}

resource "azurerm_route_table" "cluster-01" {
  name                = "${azurerm_resource_group.aks.name}-routetable"
  location            = "${azurerm_resource_group.aks.location}"
  resource_group_name = "${azurerm_resource_group.aks.name}"

  route {
    name                   = "cluster-01"
    address_prefix         = "10.100.0.0/14"
    next_hop_type          = "VirtualAppliance"
    next_hop_in_ip_address = "10.10.1.1"
  }
}

resource "azurerm_subnet_route_table_association" "cluster-01" {
  subnet_id      = "${azurerm_subnet.cluster.id}"
  route_table_id = "${azurerm_route_table.cluster-01.id}"
}

resource "azurerm_role_assignment" "sp-aks-network" {
    scope                   = "${azurerm_virtual_network.demo.id}"
    role_definition_name    = "Network Contributor"
    principal_id            = "${azuread_service_principal.sp-aks.object_id}"
}