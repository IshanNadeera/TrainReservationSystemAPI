//This Controller handles all the Trains of the train reservation system

using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using NuGet.Protocol.Plugins;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrainReservationSystem.Models;

namespace TrainReservationSystem.Controllers
{
    //Define Route
    [Route("api/[controller]")]
    [ApiController]
    public class TrainController : ControllerBase
    {
        private readonly IMongoCollection<Train> _trainCollection;

        public TrainController(IMongoDatabase database)
        {
            // Initialize the MongoDB collection used for train schedules
            _trainCollection = database.GetCollection<Train>("train_schedule");
        }

        // GET: api/Train - Get All Trains
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                // Retrieve all train schedules from the MongoDB collection
                var trains = await _trainCollection.Find(_ => true).ToListAsync();

                return Ok(trains);
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an error response
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Train/addSchedule
        [HttpPost("addSchedule")]
        public async Task<IActionResult> AddSchedule([FromBody] Train newTrain)
        {
            try
            {
                // Check if a train with the provided date and name already exists
                var existingTrain = await _trainCollection.Find(t => t.Trainname == newTrain.Trainname && t.Date == newTrain.Date).SingleOrDefaultAsync();

                if (existingTrain != null)
                {
                    return BadRequest("For this Date, Train is already added");
                }

                // Add the new train to the MongoDB collection
                await _trainCollection.InsertOneAsync(newTrain);

                return Ok("Train added successfully");
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an error response
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Train/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateTrain([FromBody] Train updatedTrain)
        {
            try
            {
                // Find the train to be updated by date and train name
                var train_filter = Builders<Train>.Filter.And(
                                        Builders<Train>.Filter.Eq(t => t.Trainname, updatedTrain.Trainname),
                                        Builders<Train>.Filter.Eq(t => t.Date, updatedTrain.Date)
                                    );

                // Check if the train exists
                var existingTrain = await _trainCollection.Find(train_filter).SingleOrDefaultAsync();

                if (existingTrain == null)
                {
                    return NotFound("Train not found");
                }

                // Update the train's information with the new data
                var update = Builders<Train>.Update
                    .Set(u => u.Trainname, updatedTrain.Trainname)
                    .Set(u => u.Date, updatedTrain.Date)
                    .Set(u => u.Starttime, updatedTrain.Starttime)
                    .Set(u => u.Endtime, updatedTrain.Endtime)
                    .Set(u => u.Description, updatedTrain.Description)
                    .Set(u => u.Noofsheet, updatedTrain.Noofsheet)
                    .Set(u => u.Ticketprice, updatedTrain.Ticketprice)
                    .Set(u => u.Status, updatedTrain.Status);

                var updateResult = await _trainCollection.UpdateOneAsync(train_filter, update);

                if (updateResult.ModifiedCount == 1)
                {
                    return Ok("Train updated successfully");
                }
                else
                {
                    return StatusCode(500, "Train update failed");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an error response
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }

}
