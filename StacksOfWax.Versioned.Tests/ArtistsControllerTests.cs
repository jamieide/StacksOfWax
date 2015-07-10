using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StacksOfWax.Shared.Models;
using StacksOfWax.Versioned;
using StacksOfWax.Versioned.Models;

namespace StacksOfWax.Versioned.Tests
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
        public void CanGetArtistsV1()
        {
            var response = _server.HttpClient.GetAsync("api/v1/artists").Result;
            response.EnsureSuccessStatusCode();
            var artists = response.Content.ReadAsAsync<IEnumerable<ArtistV1>>().Result;

            Assert.IsTrue(artists.Any());
            // Conversion is loose so we have to test that we got the correct type
            Assert.IsInstanceOfType(artists.First(), typeof(ArtistV1));
        }

        [TestMethod]
        public void CanGetArtistsV2()
        {
            var response = _server.HttpClient.GetAsync("api/v2/artists").Result;
            response.EnsureSuccessStatusCode();
            var artists = response.Content.ReadAsAsync<IEnumerable<ArtistV2>>().Result;

            Assert.IsTrue(artists.Any());
            Assert.IsInstanceOfType(artists.First(), typeof(ArtistV2));
        }


        [TestMethod]
        public void CanGetArtistsV2Albums()
        {
            var response = _server.HttpClient.GetAsync("api/v2/artists/1/albums").Result;
            response.EnsureSuccessStatusCode();
            var artists = response.Content.ReadAsAsync<IEnumerable<Album>>().Result;

            Assert.IsTrue(artists.Any());
            Assert.IsInstanceOfType(artists.First(), typeof(Album));
        }
    }
}
