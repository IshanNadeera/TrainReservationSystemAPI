//This Model store all the Trains

using System;
using System.Net.Sockets;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TrainReservationSystem.Models;

namespace TrainReservationSystem.Models
{
    //Ignore extra elements for class
    [BsonIgnoreExtraElements]

    public class Train
    {
        //Convert MongoDb ID
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        //Mapping Data - C# is Uppercase and MongoDB is lower case.
        [BsonElement("trainname")]
        public string Trainname { get; set; } = String.Empty;

        [BsonElement("date")]
        public string Date { get; set; } = String.Empty;

        [BsonElement("starttime")]
        public string Starttime { get; set; } = String.Empty;

        [BsonElement("endtime")]
        public string Endtime { get; set; } = String.Empty;

        [BsonElement("description")]
        public string Description { get; set; } = String.Empty;

        [BsonElement("noofsheet")]
        public string Noofsheet { get; set; } = String.Empty;

        [BsonElement("ticketprice")]
        public string Ticketprice { get; set; } = String.Empty;

        [BsonElement("status")]
        public string Status { get; set; } = String.Empty;
    }
}


