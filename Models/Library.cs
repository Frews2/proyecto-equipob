using System;
using System.Collections.Generic;

#nullable disable

namespace MSPublicLibrary.Models
{
    public partial class Library
    {
        public Library()
        {
            Songs = new HashSet<Song>();
        }

        public string LibraryId { get; set; }
        public string LibraryName { get; set; }
        public string AccountId { get; set; }

        public virtual ICollection<Song> Songs { get; set; }
    }
}
