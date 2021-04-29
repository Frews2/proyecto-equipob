using System;
using System.Collections.Generic;

#nullable disable

namespace MSPublicLibrary.Models
{
    public partial class Album
    {
        public Album()
        {
            Songs = new HashSet<Song>();
        }

        public string AlbumId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Song> Songs { get; set; }
    }
}
