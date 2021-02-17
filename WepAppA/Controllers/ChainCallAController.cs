using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

        public async Task<IActionResult> ChainCall(int? delayInSeconds, bool propagate = false, bool forwardHeaders = true)
        {
            //https://github.com/microsoft/mindaro/blob/6f0be147079afd923df1f0268bdcc4e24a0a8eec/samples/BikeSharingApp/Gateway/HttpHelper.cs#L21
            var url = $"{_configuration["WebAppBUrl"]}/chaincallb/chaincall?delayInSeconds={delayInSeconds.GetValueOrDefault(0)}&propagate={propagate}";
            List<ResponseModel> models = new List<ResponseModel>();
            models.Add(new ResponseModel("Service A", "Received request"));
            models.Add(new ResponseModel("Service A", " Forwarding request to Service B."));
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, url);
            if (forwardHeaders)
            {
                message = message.AddOutboundHeaders(Request);
            }
            var client = _clientFactory.CreateClient("default");
            var productResponse = await client.SendAsync(message);
            var responseBody = await productResponse.Content.ReadAsStringAsync();
            var responses = JsonConvert.DeserializeObject<List<ResponseModel>>(responseBody);
            models.AddRange(responses);
            models.Add(new ResponseModel("Service B", " Received response from Service B."));
            return Ok(models);

        }
    }
}
