using System.Linq;
using System.Web.Http;
using StacksOfWax.Shared.DataAccess;
using StacksOfWax.Versioned.Models;

namespace StacksOfWax.Versioned.Controllers
{
    public class ArtistsV1Controller : ApiController
    {
        private readonly StacksOfWaxDbContext _db;

        public ArtistsV1Controller()
        {
            _db = new StacksOfWaxDbContext();
        }

        public IHttpActionResult GetArtists()
        {
            var vm = _db.Artists
                .Select(x => new ArtistV1
                {
                    ArtistId = x.ArtistId,
                    Name = x.Name
                });

            return Ok(vm);
        }

        public IHttpActionResult GetArtist(int id)
        {
            var vm = _db.Artists
                .Where(x => x.ArtistId == id)
                .Select(x => new ArtistV1()
                {
                    ArtistId = x.ArtistId,
                    Name = x.Name
                }).SingleOrDefault();

            if (vm == null)
            {
                return NotFound();
            }

            return Ok(vm);
        }

    }
}