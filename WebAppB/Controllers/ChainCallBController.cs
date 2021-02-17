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
using Newtonsoft.Json;

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
        public async Task<ActionResult<List<ResponseModel>>> ChainCall(int? delayInSeconds, bool propagate = false)
        {
            List<ResponseModel> models = new List<ResponseModel> { new ResponseModel("Service B", "Received request") };
            await Task.Delay(delayInSeconds.GetValueOrDefault(0) * 1000);

            if (!propagate)
            {
                return Ok(models);
            }
            
            var url = $"{_configuration["WebAppCUrl"]}/chaincallc/chaincall?delayInSeconds={delayInSeconds}";
            models.Add(new ResponseModel("Service B", " Forwarding request to Service C."));
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, url).AddOutboundHeaders(Request);
            var client = _clientFactory.CreateClient("default");
            var productResponse = await client.SendAsync(message);
            var responseBody = await productResponse.Content.ReadAsStringAsync();
            var responses = JsonConvert.DeserializeObject<List<ResponseModel>>(responseBody);
            models.AddRange(responses);
            models.Add(new ResponseModel("Service B", " Received response from Service C."));
            return Ok(models);

        }
    }
}
