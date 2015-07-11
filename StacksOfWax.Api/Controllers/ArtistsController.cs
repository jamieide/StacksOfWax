using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
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
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IQueryable<Artist> GetArtists()
        {
            return _db.Artists;
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

        // POST /artists
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

        // PUT /artists/1
        public IHttpActionResult PutArtist(int id, [FromBody]Artist artist)
        {
            if (!ModelState.IsValid || id != artist.ArtistId)
            {
                return BadRequest();
            }

            // could also retrieve and set selected properties
            _db.Entry(artist).State = EntityState.Modified;
            _db.SaveChanges();
            return Ok();
        }

        // DELETE artists/1
        public IHttpActionResult DeleteArtist(int id)
        {
            var artist = _db.Artists.FirstOrDefault(x => x.ArtistId == id);
            if (artist == null)
            {
                return NotFound();
            }

            _db.Artists.Remove(artist);
            _db.SaveChanges();
            return Ok();
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
