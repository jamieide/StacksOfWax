using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StacksOfWax.Shared.Models;

namespace StacksOfWax.HmacAuth.Tests
{

    [TestClass]
    public class ArtistsControllerTests
    {
        private static TestServer _server;

        [ClassInitialize]
        public static void SetUp(TestContext context)
        {
            _server = TestServer.Create<Startup>();
        }

        [ClassCleanup]
        public static void Teardown()
        {
            _server.Dispose();
        }


        [TestMethod]
        public void CanGetArtists()
        {
            var request = _server.CreateRequest("api/artists");
            IEnumerable<Artist> artists;
            using (var response = request.GetAsync().Result)
            {
                response.EnsureSuccessStatusCode();
                artists = response.Content.ReadAsAsync<IEnumerable<Artist>>().Result;
            }

            Assert.IsTrue(artists.Any());
        }

        [TestMethod]
        public void CannotGetArtistWithoutHmac()
        {
            var request = _server.CreateRequest("api/artists/1");
            using (var response = request.GetAsync().Result)
            {
                Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }

        [TestMethod]
        public void CanGetArtistWithHmac()
        {
            var request = _server.CreateRequest("api/artists/1")
                .And(GenerateAndAddAuthorizationHeader);
            using (var response = request.GetAsync().Result)
            {
                response.EnsureSuccessStatusCode();
            }
            // TODO
        }

        private void GenerateAndAddAuthorizationHeader(HttpRequestMessage request)
        {
            // TODO more comments
            // see http://bitoftech.net/2014/12/15/secure-asp-net-web-api-using-api-key-authentication-hmac-authentication/
            // These should be stored securely
            const string appId = "2fad4c19-0fcc-429e-8f41-e21b73db75cd";
            const string apiKey = "SjSa9vT4QWNXERDcAde4rjtc4tq4ZNojjaq7JoZ+81w=";

            // TODO Should use absolute URI but can't with relative 
            var requestUri = WebUtility.UrlEncode(request.RequestUri.ToString());
            var requestMethod = request.Method.Method;

            // Calculate UNIX time
            var epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var timeSpan = DateTime.UtcNow - epochStart;
            var timeStamp = Convert.ToUInt64(timeSpan.TotalSeconds).ToString();

            var requestContent = string.Empty;
            if (request.Content != null)
            {
                var content = request.Content.ReadAsByteArrayAsync().Result;
                var md5 = MD5.Create();
                // Hash request body
                var contentHash = md5.ComputeHash(content);
                requestContent = Convert.ToBase64String(contentHash);
            }

            var hmacData = string.Concat(appId, requestMethod, requestUri, timeStamp, requestContent);
            var secretKeyByteArray = Convert.FromBase64String(apiKey);
            var signature = Encoding.UTF8.GetBytes(hmacData);

            using (var hmac = new HMACSHA256(secretKeyByteArray))
            {
                var signatureBytes = hmac.ComputeHash(signature);
                var requestSignature = Convert.ToBase64String(signatureBytes);
                var headerValue = string.Format("{0}:{1}:{2}", appId, requestSignature, timeStamp);
                request.Headers.Authorization = new AuthenticationHeaderValue("SOW", headerValue);
            }
        }

    }
}
