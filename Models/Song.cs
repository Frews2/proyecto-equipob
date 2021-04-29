using System;
using System.Collections.Generic;

#nullable disable

namespace MSPublicLibrary.Models
{
    public partial class Song
    {
        public string SongId { get; set; }
        public string Title { get; set; }
        public string LibraryId { get; set; }
        public string ArtistId { get; set; }
        public int Duration { get; set; }
        public string ReleaseYear { get; set; }
        public string Producer { get; set; }
        public string Composer { get; set; }
        public string MultimediaId { get; set; }
        public int GenreId { get; set; }
        public string AlbumId { get; set; }
        public int StatusId { get; set; }

        public virtual Album Album { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual Library Library { get; set; }
        public virtual SongStatus Status { get; set; }
    }
}
