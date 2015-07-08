using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace StacksOfWax.Shared.Models
{
    [DataContract]
    public class Artist
    {
        protected Artist()
        {
            Albums = new Collection<Album>();
        }

        public Artist(string name) : this()
        {
            Name = name;
        }

        [DataMember]
        public int ArtistId { get; private set; }

        [DataMember]
        [Required]
        public string Name { get; set; }

        public ICollection<Album> Albums { get; private set; }
    }
}
