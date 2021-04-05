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
    public class ChainCallController : ControllerBase
    {
        private readonly ILogger<ChainCallController> _logger;
        private readonly IConfiguration _configuration;
        private readonly System.Net.Http.IHttpClientFactory _clientFactory;


        public ChainCallController(ILogger<ChainCallController> logger, IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var responseBody = $"Working. Chaincall method - chaincall?delayInSeconds=0&propagate=false&forwardHeaders=true";
            return Ok(responseBody);
        }

        [HttpGet("ChainCall")]

        public async Task<IActionResult> ChainCall(int? delayInSeconds, bool propagate = false, bool forwardHeaders = true)
        {
            var url = $"{_configuration["WebAppBUrl"]}/chaincallb/chaincall?delayInSeconds={delayInSeconds.GetValueOrDefault(0)}&propagate={propagate}";
            List<ResponseModel> models = new List<ResponseModel>();
            models.Add(new ResponseModel("Service A", "Received request"));
            models.Add(new ResponseModel("Service A", $" Forwarding request to Service B at url {url}."));

            try
            {
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
                models.Add(new ResponseModel("Service A", " Received response from Service B."));
            }
            catch (Exception e)
            {
                models.Add(new ResponseModel("Service A Error", e.ToString()));
            }
            //https://github.com/microsoft/mindaro/blob/6f0be147079afd923df1f0268bdcc4e24a0a8eec/samples/BikeSharingApp/Gateway/HttpHelper.cs#L21
            
            
            
            return Ok(models);

        }
    }
}
