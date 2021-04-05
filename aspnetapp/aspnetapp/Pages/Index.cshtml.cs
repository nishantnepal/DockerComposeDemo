using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace aspnetapp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private IConfiguration _configuration;
        public string PathBaseVersion { get; set; }
        public string AppEnvironment { get; set; }

        public IndexModel(ILogger<IndexModel> logger,IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void OnGet()
        {
            PathBaseVersion = _configuration["VersionPathBase"];
            AppEnvironment = _configuration["AppEnvironment"];
            if(string.IsNullOrWhiteSpace(AppEnvironment)){
                AppEnvironment = "n/a";
            }

            //var httpRequestFeature = Request.HttpContext.Features.Get<IHttpRequestFeature>();
            //RawRequestUrl = httpRequestFeature.RawTarget;
        }

        /*
         * public static string GetRawTarget(this HttpRequest request)
        {
            var httpRequestFeature = request.HttpContext.Features.Get<IHttpRequestFeature>(); 
            return httpRequestFeature.RawTarget;

        }

         */
    }
}
