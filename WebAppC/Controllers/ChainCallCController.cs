using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
            var responseBody = $"Ok. Append this to the url to call the chaincall method - chaincall?name=john";
            return Ok(responseBody);
        }

        [HttpGet("ChainCall")]

        public IActionResult ChainCall(string name)
        {
            var responseBody = $"Hello {name} from App C being called at {Request.GetDisplayUrl()}";
            return Ok(responseBody);
        }
    }
}
