using System;

namespace MSPrivateLibrary.Models
{
    public class PrivateLibraryDatabaseSettings : IPrivateLibraryDatabaseSettings
    {
        public string ConnectionString { get; set; } = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        public string MusicCollectionName { get; set; }
        public string SongCollectionName { get; set; }
        public string AlbumCollectionName { get; set; }
        public string GenreCollectionName { get; set; }
        public string SongStatusCollectionName { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IPrivateLibraryDatabaseSettings
    {
        string MusicCollectionName { get; set; }
        string SongCollectionName { get; set; }
        string AlbumCollectionName { get; set; }
        string GenreCollectionName { get; set; }
        string SongStatusCollectionName { get; set; }
        string DatabaseName { get; set; }
    }
}