using System;
using System.Collections.Generic;

#nullable disable

namespace MSPublicLibrary.Models
{
    public partial class Genre
    {
        public Genre()
        {
            Songs = new HashSet<Song>();
        }

        public int GenreId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Song> Songs { get; set; }
    }
}
