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
    public class UserController : ControllerBase
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserController(IMongoDatabase database)
        {
            _userCollection = database.GetCollection<User>("user");
        }

        // GET: api/User - Get All Users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userCollection.Find(_ => true).ToListAsync();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/User/login
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var user = await _userCollection.Find(u => u.Nic == loginRequest.NIC).SingleOrDefaultAsync();

                if (user == null)
                {
                    return NotFound("User not found");
                }

                if (user.Password != loginRequest.Password)
                {
                    return Unauthorized("Incorrect password");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/user/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] User newUser)
        {
            try
            {
                // Check if a user with the provided NIC already exists
                var existingUser = await _userCollection.Find(u => u.Nic == newUser.Nic).SingleOrDefaultAsync();

                if (existingUser != null)
                {
                    return BadRequest("User with this NIC is already registered");
                }

                // Add the new user to the MongoDB collection
                await _userCollection.InsertOneAsync(newUser);

                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/user/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] User updatedUser)
        {
            try
            {
                // Find the user to be updated by NIC
                var nic_filter = Builders<User>.Filter.Eq(u => u.Nic, updatedUser.Nic);

                // Check if the user exists
                var existingUser = await _userCollection.Find(nic_filter).SingleOrDefaultAsync();

                if (existingUser == null)
                {
                    return NotFound("User not found");
                }

                // Update the user's information with the new data
                var update = Builders<User>.Update
                    .Set(u => u.Fname, updatedUser.Fname)
                    .Set(u => u.Lname, updatedUser.Lname)
                    .Set(u => u.Nic, updatedUser.Nic)
                    .Set(u => u.Phone_no, updatedUser.Phone_no)
                    .Set(u => u.Status, updatedUser.Status)
                    .Set(u => u.Email, updatedUser.Email);

                var updateResult = await _userCollection.UpdateOneAsync(nic_filter, update);

                if (updateResult.ModifiedCount == 1)
                {
                    return Ok("User updated successfully");
                }
                else
                {
                    return StatusCode(500, "User update failed");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class LoginRequest
    {
        public string NIC { get; set; } =  String.Empty;
        public string Password { get; set; } = String.Empty;
    }
}
