using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace StacksOfWax.Web.Infrastructure
{
    /// <summary>
    /// Factory for StacksOfWaxClient
    /// </summary>
    public static class StacksOfWaxClientFactory
    {
        public static HttpClient GetClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:56297/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}