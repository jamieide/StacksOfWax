using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StacksOfWax.Shared.Models;

namespace StacksOfWax.Owin.Tests
{
    /// <summary>
    /// These unit tests demonstrate in-memory testing of an OWIN enabled Web API.
    /// </summary>
    [TestClass]
    public class ArtistsControllerInMemoryTests
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

        [TestMethod]
        public void CanGetArtists()
        {
            // Act
            var response = _server.HttpClient.GetAsync("api/artists").Result;
            response.EnsureSuccessStatusCode();
            var artists = response.Content.ReadAsAsync<IEnumerable<Artist>>().Result;

            // Assert
            Assert.IsNotNull(artists);
            Assert.IsTrue(artists.Any());
        }

        [TestMethod]
        public async Task CanGetArtistsAsync()
        {
            // Act
            var response = await _server.HttpClient.GetAsync("api/artists");
            response.EnsureSuccessStatusCode();
            var artists = await response.Content.ReadAsAsync<IEnumerable<Artist>>();

            // Assert
            Assert.IsNotNull(artists);
            Assert.IsTrue(artists.Any());
        }

        [TestMethod]
        public void CanGetArtist()
        {
            // Act
            var response = _server.HttpClient.GetAsync("api/artists/1").Result;
            response.EnsureSuccessStatusCode();
            var artist = response.Content.ReadAsAsync<Artist>().Result;

            // Assert
            Assert.IsNotNull(artist);
            Assert.AreEqual(1, artist.ArtistId);
        }
    }
}