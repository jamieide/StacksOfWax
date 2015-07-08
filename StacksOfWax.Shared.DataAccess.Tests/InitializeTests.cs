using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StacksOfWax.Shared.DataAccess.Tests
{
    [TestClass]
    public class InitializeTests
    {
        [TestMethod]
        public void CanInitialize()
        {
            using (var db = new StacksOfWaxDbContext())
            {
                Assert.IsTrue(db.Artists.Any());
                Assert.IsTrue(db.Albums.Any());
            }
        }
    }
}