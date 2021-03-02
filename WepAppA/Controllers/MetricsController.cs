using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;

namespace WepAppA.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MetricsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public MetricsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public Metrics Get()
        {
            return new Metrics()
            {
                PathBaseVersion = _configuration["VersionPathBase"],
                RequestPath = this.Request.GetDisplayUrl(),
                CPUCores = Environment.ProcessorCount.ToString(),
                MachineName = Environment.MachineName,
                AppEnvironment = _configuration["AppEnvironment"]
            };

        }

    }

    public class Metrics
    {
        public string PathBaseVersion { get; set; }
        public string RequestPath { get; set; }
        public string CPUCores { get; set; }
        public string MachineName { get; set; }
        public string OSDescription { get; set; }
        public string AppEnvironment { get; set; }
    }
}
