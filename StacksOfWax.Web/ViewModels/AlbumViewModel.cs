using System.ComponentModel.DataAnnotations;

namespace StacksOfWax.Web.ViewModels
{
    public class AlbumViewModel
    {
        public int AlbumId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }
    }
}