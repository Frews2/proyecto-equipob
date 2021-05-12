/*
 Date: 05/05/2021
 Author(s): Ricardo Moguel Sanchez
*/
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MSPrivateLibrary.Models{
    public class Album
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ObjectId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageAddress { get; set; }
        public string ImageName { get; set; }
        public string ImageType { get; set; }
    }
}