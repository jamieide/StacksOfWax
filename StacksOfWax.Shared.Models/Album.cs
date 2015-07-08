using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace StacksOfWax.Shared.Models
{
    [DataContract]
    public class Album
    {
        protected Album()
        {}

        public Album(Artist artist, string name) : this()
        {
            Artist = artist;
            Name = name;
        }

        [DataMember]
        public int AlbumId { get; private set; }

        [DataMember]
        [Required]
        public string Name { get; set; }

        public int ArtistId { get; set; }
        
        public Artist Artist { get; set; }

    }
}