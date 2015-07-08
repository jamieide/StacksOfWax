using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using StacksOfWax.Shared.DataAccess;
using StacksOfWax.Shared.Models;

namespace StacksOfWax.WebApiTemplate.Controllers
{
    public class AlbumsController : ApiController
    {
        private readonly StacksOfWaxDbContext _db;

        public AlbumsController() : this(new StacksOfWaxDbContext())
        {}

        public AlbumsController(StacksOfWaxDbContext db)
        {
            _db = db;
        }

        // GET: api/Albums
        public IQueryable<Album> GetAlbums()
        {
            return _db.Albums;
        }

        // GET: api/Albums/5
        [ResponseType(typeof(Album))]
        public IHttpActionResult GetAlbum(int id)
        {
            Album album = _db.Albums.Find(id);
            if (album == null)
            {
                return NotFound();
            }

            return Ok(album);
        }

        // PUT: api/Albums/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAlbum(int id, Album album)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != album.AlbumId)
            {
                return BadRequest();
            }

            _db.Entry(album).State = EntityState.Modified;

            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Albums
        [ResponseType(typeof(Album))]
        public IHttpActionResult PostAlbum(Album album)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Albums.Add(album);
            _db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = album.AlbumId }, album);
        }

        // DELETE: api/Albums/5
        [ResponseType(typeof(Album))]
        public IHttpActionResult DeleteAlbum(int id)
        {
            Album album = _db.Albums.Find(id);
            if (album == null)
            {
                return NotFound();
            }

            _db.Albums.Remove(album);
            _db.SaveChanges();

            return Ok(album);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AlbumExists(int id)
        {
            return _db.Albums.Count(e => e.AlbumId == id) > 0;
        }
    }
}