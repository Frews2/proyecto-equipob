using System;
using System.Collections.Generic;

#nullable disable

namespace MSPublicLibrary.Models
{
    public partial class SongStatus
    {
        public SongStatus()
        {
            Songs = new HashSet<Song>();
        }

        public int StatusId { get; set; }
        public string Status { get; set; }

        public virtual ICollection<Song> Songs { get; set; }
    }
}
