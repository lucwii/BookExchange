using BookExchange.Models;
using MongoDB.Driver;

namespace BookExchange.Services;

public class MongoDbService
{
    private readonly IMongoDatabase _db;

    public MongoDbService(IConfiguration config)
    {
        var client = new MongoClient(config["MongoDB:ConnectionString"]);
        _db = client.GetDatabase(config["MongoDB:DatabaseName"]);
    }

    public IMongoCollection<User> Users =>
        _db.GetCollection<User>("users");

    public IMongoCollection<Book> Books =>
        _db.GetCollection<Book>("books");

    public IMongoCollection<ExchangeRequest> ExchangeRequest =>
        _db.GetCollection<ExchangeRequest>("requests");
}