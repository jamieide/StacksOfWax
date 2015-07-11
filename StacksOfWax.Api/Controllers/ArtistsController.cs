using System.Linq;
using System.Runtime.InteropServices;
using System.Web.Http;
using StacksOfWax.Shared.DataAccess;
using StacksOfWax.Shared.Models;

namespace StacksOfWax.Api.Controllers
{
    public class ArtistsController : ApiController
    {
        private readonly StacksOfWaxDbContext _db;

        public ArtistsController()
        {
            _db = new StacksOfWaxDbContext();
        }

        // GET /artists
        public IHttpActionResult GetArtists()
        {
            var artists = _db.Artists.ToList();
            return Ok(artists);
        }

        // GET /artists/1
        public IHttpActionResult GetArtist(int id)
        {
            var artist = _db.Artists.SingleOrDefault(x => x.ArtistId == id);
            if (artist == null)
            {
                return NotFound();
            }
            return Ok(artist);
        }

        public IHttpActionResult PostArtist(Artist artist)
        {
            // TODO artist name must be unique
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Artists.Add(artist);
            _db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new {id = artist.ArtistId}, artist);
        }

        [Route("api/artists/{id}/albums")]
        public IHttpActionResult GetArtistAlbums(int id)
        {
            var albums = _db.Albums
                .Where(x => x.ArtistId == id)
                .OrderBy(x => x.Name)
                .ToList();

            return Ok(albums);
        }
    }
}
