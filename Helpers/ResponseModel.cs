using System;
using System.Collections.Generic;
using System.Text;

namespace Helpers
{
    public class ResponseModel
    {
        public string ServiceName { get; set; }
        public string NodeName { get; }
        public string PodName { get;  }
        public string PodNamespace { get; }
        public string PodIp { get;  }
        public DateTime DateTimeUtc { get; }
        public string LogText { get; set; }

        public ResponseModel(string serviceName,string logText)
        {
            ServiceName = serviceName;
            LogText = logText;
            NodeName = Environment.GetEnvironmentVariable(Constants.ENV_MY_NODE_NAME);
            PodName = Environment.GetEnvironmentVariable(Constants.ENV_MY_POD_NAME);
            PodNamespace = Environment.GetEnvironmentVariable(Constants.ENV_MY_POD_NAMESPACE);
            PodIp = Environment.GetEnvironmentVariable(Constants.ENV_MY_POD_IP);
            DateTimeUtc = DateTime.UtcNow;
        }
    }
}
