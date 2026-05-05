using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookExchange.Models;

public class ExchangeRequest
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string KnjigaId { get; set; } = string.Empty;
    public string KnjigaNaslov { get; set; } = string.Empty;
    public string PosiljacId { get; set; } = string.Empty;
    public string PosiljacIme { get; set; } = string.Empty;
    public string VlasnikId { get; set; } = string.Empty;
    public DateTime DatumSlanja { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Na čekanju";
    public bool PosiljacObavesten { get; set; } = false;
}