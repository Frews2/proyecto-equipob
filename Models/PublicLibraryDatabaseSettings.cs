using System;

namespace MSPublicLibrary.Models
{
    public class PublicLibraryDatabaseSettings : IPublicLibraryDatabaseSettings
    {
        public string ConnectionString { get; set; } = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        public string MusicCollectionName { get; set; }
        public string SongCollectionName { get; set; }
        public string AlbumCollectionName { get; set; }
        public string GenreCollectionName { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IPublicLibraryDatabaseSettings
    {
        string MusicCollectionName { get; set; }
        string SongCollectionName { get; set; }
        string AlbumCollectionName { get; set; }
        string GenreCollectionName { get; set; }
        string DatabaseName { get; set; }
    }
}