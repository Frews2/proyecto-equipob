/*
 Date: 05/05/2021
 Author(s): Ricardo Moguel Sanchez
*/
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MSPrivateLibrary.Models{
    public class Music
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ObjectId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public string Address { get; set; }
        public Decimal Size { get; set; }
        public DateTime CreationDate { get; set; }
    }
}