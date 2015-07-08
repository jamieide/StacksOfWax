using System.Linq;
using System.Web.Http;
using StacksOfWax.Shared.DataAccess;
using StacksOfWax.Versioned.Models;

namespace StacksOfWax.Versioned.Controllers
{
    public class ArtistsV2Controller : ApiController
    {
        private readonly StacksOfWaxDbContext _db;

        public ArtistsV2Controller()
        {
            _db = new StacksOfWaxDbContext();
        }

        public IHttpActionResult GetArtists()
        {
            var vm = _db.Artists
                .Select(x => new ArtistV2
                {
                    ArtistId = x.ArtistId,
                    Name = x.Name,
                    AlbumCount = x.Albums.Count
                });

            return Ok(vm);
        }

        public IHttpActionResult GetArtist(int id)
        {
            var vm = _db.Artists
                .Where(x => x.ArtistId == id)
                .Select(x => new ArtistV2
                {
                    ArtistId = x.ArtistId,
                    Name = x.Name,
                    AlbumCount = x.Albums.Count
                }).SingleOrDefault();

            if (vm == null)
            {
                return NotFound();
            }

            return Ok(vm);
        }

    }
}