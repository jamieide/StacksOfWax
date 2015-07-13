using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StacksOfWax.Shared.Models;

namespace StacksOfWax.BasicAuth.Tests
{
    [TestClass]
    public class ArtistsControllerTests
    {
        private static TestServer _server;

        [ClassInitialize]
        public static void SetUp(TestContext context)
        {
            // Arrange
            _server = TestServer.Create<Startup>();
        }

        [ClassCleanup]
        public static void TearDown()
        {
            _server.Dispose();
        }

        private RequestBuilder CreateRequestWithCredentials(string path)
        {
            var request = _server.CreateRequest(path);
            
            var auth = Convert.ToBase64String(Encoding.ASCII.GetBytes("tester:secret"));
            var credentials = "Basic " + auth;
            request.AddHeader("Authorization", credentials);
            return request;
        }

        [TestMethod]
        public void CanGetArtistsWithoutCredentials()
        {
            var request = _server.CreateRequest("api/artists");
            var response = request.GetAsync().Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
            var artists = response.Content.ReadAsAsync<IEnumerable<Artist>>().Result;

            Assert.IsNotNull(artists);
            Assert.IsTrue(artists.Any());
        }

        [TestMethod]
        public void CannotGetArtistWithoutCredentials()
        {
            var request = _server.CreateRequest("api/artists/1");
            var response = request.GetAsync().Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public void CanGetArtistWithCredentials()
        {
            var request = CreateRequestWithCredentials("api/artists/1");
            var response = request.GetAsync().Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
            var artist = response.Content.ReadAsAsync<Artist>().Result;

            Assert.IsNotNull(artist);
        }


    }
}
