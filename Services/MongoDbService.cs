using BookExchange.Models;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace BookExchange.Services;

public class MongoDbService
{
    private readonly IMongoDatabase _db;
    public readonly IGridFSBucket GridFS;
    
    public MongoDbService(IConfiguration config)
    {
        var client = new MongoClient(config["MongoDB:ConnectionString"]);
        _db = client.GetDatabase(config["MongoDB:DatabaseName"]);
        GridFS = new GridFSBucket(_db);
    }

    public IMongoCollection<User> Users =>
        _db.GetCollection<User>("users");

    public IMongoCollection<Book> Books =>
        _db.GetCollection<Book>("books");

    public IMongoCollection<ExchangeRequest> ExchangeRequest =>
        _db.GetCollection<ExchangeRequest>("requests");
}