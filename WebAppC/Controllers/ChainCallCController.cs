﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Helpers;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebAppC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChainCallCController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ChainCallCController> _logger;
        private readonly IConfiguration _configuration;
        private readonly System.Net.Http.IHttpClientFactory _clientFactory;
        private HttpClient _client;


        public ChainCallCController(ILogger<ChainCallCController> logger, IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _clientFactory = clientFactory;
            //_client = client;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var responseBody = $"{Environment.MachineName}: Append this to the url to call the chaincall method - /chaincall";
            return Ok(responseBody);
        }

        [HttpGet("ChainCall")]

        public async Task<IActionResult> ChainCall(int? delayInSeconds)
        {
            List<ResponseModel> models = new List<ResponseModel> {new ResponseModel("Service C", " Received request")};
            await Task.Delay(delayInSeconds.GetValueOrDefault(0) * 1000);
            return Ok(models);
        }

        [HttpGet("filemountedconfig")]

        public async Task<IActionResult> FileMountedconfig(string configKey)
        {
            var configValue = _configuration[configKey];
            return Ok(new
            {
                configKey,
                configValue
            });
        }

        [HttpGet("configvalue")]

        public async Task<IActionResult> GetConfigValue(string configKey)
        {
            var configValue = _configuration[configKey];
            return Ok(new
            {
                configKey,
                configValue
            });
        }
    }
}
