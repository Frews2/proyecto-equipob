/*
 Date: 05/05/2021
 Author(s): Ricardo Moguel Sanchez
*/
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MSPublicLibrary.Models{
    public class Song
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ObjectId { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string ArtistId { get; set; }
        public string MultimediaId { get; set; }
        public string Composer { get; set; }
        public string Producer { get; set; }
        public string Duration { get; set; }
        public string ReleaseYear { get; set; }
        public string AlbumId { get; set; }
        public string GenreId { get; set; }

    }
}