﻿using System.Linq;
using System.Web.Http;
using StacksOfWax.Shared.DataAccess;

namespace StacksOfWax.BasicAuth.Controllers
{
    public class ArtistsController : ApiController
    {
        private readonly StacksOfWaxDbContext _db;

        public ArtistsController()
        {
            _db = new StacksOfWaxDbContext();
        }

        [AllowAnonymous]
        public IHttpActionResult GetArtists()
        {
            return Ok(_db.Artists);
        }

        [Authorize]
        public IHttpActionResult GetArtist(int id)
        {
            var artist = _db.Artists.SingleOrDefault(x => x.ArtistId == id);
            if (artist == null)
            {
                return NotFound();
            }

            return Ok(artist);
        }
    }
}