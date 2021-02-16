using System.Net.Http;
using Microsoft.AspNetCore.Http;
namespace Helpers
{
    public static class HttpClientExtensions
    {
        public static HttpRequestMessage AddOutboundHeaders(this HttpRequestMessage message, HttpRequest originRequest)
        {
            // message.Headers.Add(Constants.RequestIdHeaderName, OperationContext.CurrentContext.RequestId.ToString());
            message.Headers.Add(Constants.KubernetesRouteAsHeaderName, originRequest.Headers[Constants.KubernetesRouteAsHeaderName].ToArray());
            return message;
        }
    }

    public class Constants
    {
        public const string RequestIdHeaderName = "x-contoso-request-id";

        public const string KubernetesRouteAsHeaderName = "kubernetes-route-as";
    }
}
