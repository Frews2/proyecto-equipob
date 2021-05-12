/*
 Date: 11/05/2021
 Author(s): Ricardo Moguel Sanchez
*/
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MSPrivateLibrary.Models{
    public class SongStatus
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ObjectId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}