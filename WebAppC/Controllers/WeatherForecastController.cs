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
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _configuration;
        private readonly System.Net.Http.IHttpClientFactory _clientFactory;
        private HttpClient _client;


        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpClientFactory clientFactory, IConfiguration configuration)
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

        public async Task<IActionResult> ChainCall(string name)
        {
            //return Ok($"Hello {name} from App C being called at {Request.GetDisplayUrl()}");
            var url = $"{_configuration["WebAppBUrl"]}/weatherforecast/chaincall?name={name}";
            var client = _clientFactory.CreateClient("default");
            var productResponse = await client.GetAsync(url);
            var reponseBody = await productResponse.Content.ReadAsStringAsync();
            var responses = new List<string>()
            {
                reponseBody,
                $"Hello {name} from App C being called at {Request.GetDisplayUrl()}"
            };

            return Ok(responses);
        }
    }
}
