using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StacksOfWax.Shared.Models;

namespace StacksOfWax.Owin.Tests
{
    /// <summary>
    /// These unit tests demonstrate self-hosting an OWIN enabled Web API. However, for unit testing purposes, in-memory hosting is recommended.
    /// I am unable to set a breakpoint using self-hosting.
    /// </summary>
    [TestClass]
    public class ArtistsControllerSelfHostTests
    {
        private static IDisposable _webApp;

        [ClassInitialize]
        public static void SetUp(TestContext context)
        {
            _webApp = WebApp.Start<Startup>("http://localhost:55555/");
        }

        [ClassCleanup]
        public static void TearDown()
        {
            _webApp.Dispose();
        }

        [TestMethod]
        public void CanGetArtists()
        {
            using (var client = new HttpClient())
            {
                // Arrange
                var requestUri = new Uri("http://localhost:55555/api/artists");

                // Act
                var result = client.GetAsync(requestUri).Result;
                result.EnsureSuccessStatusCode();
                var artists = result.Content.ReadAsAsync<IEnumerable<Artist>>().Result;

                // Assert
                Assert.IsNotNull(artists);
                Assert.IsTrue(artists.Any());
            }
        }

        [TestMethod]
        public async Task CanGetArtistsAsync()
        {
            using (var client = new HttpClient())
            {
                // Arrange
                var requestUri = new Uri("http://localhost:55555/api/artists");

                // Act
                var result = await client.GetAsync(requestUri);
                result.EnsureSuccessStatusCode();
                var artists = await result.Content.ReadAsAsync<IEnumerable<Artist>>();

                // Assert
                Assert.IsNotNull(artists);
                Assert.IsTrue(artists.Any());
            }
        }

        [TestMethod]
        public void CanGetArtist()
        {
            using (var client = new HttpClient())
            {
                // Arrange
                var requestUri = new Uri("http://localhost:55555/api/artists/1");

                // Act
                var result = client.GetAsync(requestUri).Result;
                result.EnsureSuccessStatusCode();
                var artist = result.Content.ReadAsAsync<Artist>().Result;

                // Assert
                Assert.IsNotNull(artist);
                // TODO this fails because ArtistId has a private setter.
                Assert.AreEqual(1, artist.ArtistId);
            }
        }

    }
}