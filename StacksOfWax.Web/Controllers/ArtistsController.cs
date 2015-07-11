using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using StacksOfWax.Web.Infrastructure;
using StacksOfWax.Web.ViewModels;

namespace StacksOfWax.Web.Controllers
{
    public class ArtistsController : Controller
    {

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            using (var client = StacksOfWaxClientFactory.GetClient())
            {
                var response = await client.GetAsync("artists");
                if (response.IsSuccessStatusCode)
                {
                    var artists = await response.Content.ReadAsAsync<IEnumerable<ArtistViewModel>>();
                    return View(artists);
                }
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            using (var client = StacksOfWaxClientFactory.GetClient())
            {
                var artistResponse = client.GetAsync("artists/" + id);
                var albumsResponse = client.GetAsync("artists/" + id + "/albums");

                await Task.WhenAll(artistResponse, albumsResponse);
                if (artistResponse.Result.IsSuccessStatusCode && albumsResponse.Result.IsSuccessStatusCode)
                {
                    var artist = artistResponse.Result.Content.ReadAsAsync<ArtistViewModel>();
                    var albums = albumsResponse.Result.Content.ReadAsAsync<IEnumerable<AlbumViewModel>>();
                    await Task.WhenAll(artist, albums);

                    var vm = new ArtistDetailsViewModel()
                    {
                        Artist = artist.Result,
                        Albums = albums.Result
                    };
                    return View(vm);
                }
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
            
        [HttpGet]
        public ActionResult Create()
        {
            var vm = new ArtistViewModel();
            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ArtistViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            using (var client = StacksOfWaxClientFactory.GetClient())
            {
                var response = await client.PostAsJsonAsync("artists", vm);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
    }
}