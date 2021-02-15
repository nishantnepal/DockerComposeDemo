using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WepAppA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChainCallAController : ControllerBase
    {
        private readonly ILogger<ChainCallAController> _logger;
        private readonly IConfiguration _configuration;
        private readonly System.Net.Http.IHttpClientFactory _clientFactory;


        public ChainCallAController(ILogger<ChainCallAController> logger, IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var responseBody = $"Ok. Append this to the url to call the chaincall method - chaincall?name=john";
            return Ok(responseBody);
        }

        [HttpGet("ChainCall")]

        public async Task<IActionResult> ChainCall(string name)
        {
            var url = $"{_configuration["WebAppBUrl"]}/weatherforecast/chaincall?name={name}";
            var client = _clientFactory.CreateClient("default");
            var productResponse = await client.GetAsync(url);
            var reponseBody = await productResponse.Content.ReadAsStringAsync();
            return Ok(reponseBody);
        }
    }
}
