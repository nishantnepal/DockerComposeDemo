using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Helpers;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebAppB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChainCallBController : ControllerBase
    {
        private readonly ILogger<ChainCallBController> _logger;
        private readonly IConfiguration _configuration;
        private readonly System.Net.Http.IHttpClientFactory _clientFactory;


        public ChainCallBController(ILogger<ChainCallBController> logger, IHttpClientFactory clientFactory, IConfiguration configuration)
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

        public async Task<IActionResult> ChainCall(string name, bool propagate = false)
        {
            if (!propagate)
            {
                return Ok($"Hello {name} from App B being called at {Request.GetDisplayUrl()}");
            }
            
            var url = $"{_configuration["WebAppCUrl"]}/chaincallc/chaincall?name={name}";
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, url).AddOutboundHeaders(Request);
            var client = _clientFactory.CreateClient("default");
            var productResponse = await client.SendAsync(message);
            var reponseBody = await productResponse.Content.ReadAsStringAsync();
            return Ok(reponseBody);

        }
    }
}
