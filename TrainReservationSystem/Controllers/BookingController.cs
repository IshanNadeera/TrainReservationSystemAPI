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
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IMongoCollection<Book> _bookingCollection;

        public BookingController(IMongoDatabase database)
        {
            _bookingCollection = database.GetCollection<Book>("train_booking");
        }

        // GET: api/Booking - Get All Bookings
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var bookings = await _bookingCollection.Find(_ => true).ToListAsync();

                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Booking/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateBooking([FromBody] Book updatedBooking)
        {
            try
            {
                // Find the booking to be updated by nic, booking date and train name
                var booking_filter = Builders<Book>.Filter.And(
                                        Builders<Book>.Filter.Eq(b => b.Trainname, updatedBooking.Trainname),
                                        Builders<Book>.Filter.Eq(b => b.Bookingdate, updatedBooking.Bookingdate),
                                        Builders<Book>.Filter.Eq(b => b.Nic, updatedBooking.Nic)
                                    );

                // Check if the booking exists
                var existingBooking = await _bookingCollection.Find(booking_filter).SingleOrDefaultAsync();

                if (existingBooking == null)
                {
                    return NotFound("Booking not found");
                }

                // Update the booking information with the new data
                var update = Builders<Book>.Update
                    .Set(b => b.Nic, updatedBooking.Nic)
                    .Set(b => b.Trainname, updatedBooking.Trainname)
                    .Set(b => b.Bookingdate, updatedBooking.Bookingdate)
                    .Set(b => b.Name, updatedBooking.Name)
                    .Set(b => b.Nooftickets, updatedBooking.Nooftickets)
                    .Set(b => b.Reservationdate, updatedBooking.Reservationdate)
                    .Set(b => b.Status, updatedBooking.Status);

                var updateResult = await _bookingCollection.UpdateOneAsync(booking_filter, update);

                if (updateResult.ModifiedCount == 1)
                {
                    return Ok("Booking updated successfully");
                }
                else
                {
                    return StatusCode(500, "Booking update failed");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Booking/addBooking
        [HttpPost("addBooking")]
        public async Task<IActionResult> AddBooking([FromBody] Book newBooking)
        {
            try
            {

                // Check if a booking with the same nic, trainname, and reservationdate already exists
                var existingBooking = await _bookingCollection.Find(b =>
                    b.Nic == newBooking.Nic &&
                    b.Trainname == newBooking.Trainname &&
                    b.Reservationdate == newBooking.Reservationdate
                ).FirstOrDefaultAsync();

                if (existingBooking != null)
                {
                    return BadRequest("A booking with the same NIC, train, and reservation date already exists.");
                }

                // If no existing booking is found, proceed to add the new booking to the MongoDB collection
                await _bookingCollection.InsertOneAsync(newBooking);

                return Ok("Booking added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }

}
