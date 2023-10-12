//This Model store all the Bookings

using System;
using System.Net.Sockets;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TrainReservationSystem.Models;

namespace TrainReservationSystem.Models
{
    //Ignore extra elements for class
    [BsonIgnoreExtraElements]

    public class Book
    {
        //Convert MongoDb ID
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        //Mapping Data - C# is Uppercase and MongoDB is lower case.
        [BsonElement("nic")]
        public string Nic { get; set; } = String.Empty;

        [BsonElement("trainname")]
        public string Trainname { get; set; } = String.Empty;

        [BsonElement("bookingdate")]
        public string Bookingdate { get; set; } = String.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = String.Empty;

        [BsonElement("nooftickets")]
        public string Nooftickets { get; set; } = String.Empty;

        [BsonElement("reservationdate")]
        public string Reservationdate { get; set; } = String.Empty;

        [BsonElement("status")]
        public string Status { get; set; } = String.Empty;

    }
}