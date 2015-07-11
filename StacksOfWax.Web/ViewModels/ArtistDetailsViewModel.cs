using System.Collections.Generic;

namespace StacksOfWax.Web.ViewModels
{
    public class ArtistDetailsViewModel
    {
        public ArtistViewModel Artist { get; set; }
        public IEnumerable<AlbumViewModel> Albums { get; set; }
    }
}