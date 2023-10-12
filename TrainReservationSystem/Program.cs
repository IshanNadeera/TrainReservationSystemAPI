using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using TrainReservationSystem.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure MongoDB connection
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
var database = mongoClient.GetDatabase(mongoDbSettings.DatabaseName);

// Register the MongoDB client and database in the dependency injection container
builder.Services.AddSingleton<IMongoClient>(mongoClient);
builder.Services.AddSingleton<IMongoDatabase>(database);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add CORS middleware to allow requests from specified origins
app.UseCors(options =>
{
    options.WithOrigins("http://127.0.0.1") // Replace with your allowed origin(s)
           .AllowAnyHeader()
           .AllowAnyMethod();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

