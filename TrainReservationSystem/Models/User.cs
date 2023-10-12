using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TrainReservationSystem.Models
{
    //Ignore extra elements for class
    [BsonIgnoreExtraElements]

    public class User
	{
        //Convert MongoDb ID
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        //Mapping Data - C# is Uppercase and MongoDB is lower case.
        [BsonElement("fname")]
        public string Fname { get; set; } = String.Empty;

        [BsonElement("lname")]
        public string Lname { get; set; } = String.Empty;

        [BsonElement("nic")]
        public string Nic { get; set; } = String.Empty;

        [BsonElement("phone_no")]
        public string Phone_no { get; set; } = String.Empty;

        [BsonElement("status")]
        public string Status { get; set; } = String.Empty;

        [BsonElement("role")]
        public string Role { get; set; } = String.Empty;

        [BsonElement("password")]
        public string Password { get; set; } = String.Empty;

        [BsonElement("email")]
        public string Email { get; set; } = String.Empty;
    }
}

