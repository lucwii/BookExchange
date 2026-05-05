using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookExchange.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Ime { get; set; } = string.Empty;
    public string Prezime { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string LozinkaHash { get; set; } = string.Empty;
    public string Grad { get; set; } = string.Empty;
    public List<string> OmiljeniZanrovi { get; set; } = new();
}