using System.ComponentModel.DataAnnotations;

namespace StacksOfWax.Web.ViewModels
{
    public class ArtistViewModel
    {
        public int ArtistId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }
    }
}