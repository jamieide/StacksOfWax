using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StacksOfWax.Shared.DataAccess;
using StacksOfWax.Shared.Models;
using StacksOfWax.WebApiTemplate.Controllers;

namespace StacksOfWax.WebApiTemplate.Tests
{
    [TestClass]
    public class ArtistsControllerTests
    {
        [TestMethod]
        public void CanGetArtists()
        {
            using (var db = new StacksOfWaxDbContext())
            {
                // Arrange
                var controller = new ArtistsController(db);

                // Act
                var result = controller.GetArtists() as IEnumerable<Artist>;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Any());
            }
        }

        [TestMethod]
        public void CanGetArtist()
        {
            using (var db = new StacksOfWaxDbContext())
            {
                // Arrange
                var controller = new ArtistsController(db);

                // Act
                var result = controller.GetArtist(1) as OkNegotiatedContentResult<Artist>;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Content);
                Assert.AreEqual(1, result.Content.ArtistId);
            }
        }
    }
}
