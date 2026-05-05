using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookExchange.Models;

public class Book
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Naslov { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public string Zanr { get; set; } = string.Empty;
    public bool IsNovo { get; set; }
    public string Opis { get; set; } = string.Empty;
    public string SlikaPath { get; set; } = string.Empty;
    public string VlasnikId { get; set; } = string.Empty;
    public string VlasnikIme { get; set; } = string.Empty;
    public string VlasnikEmail { get; set; } = string.Empty;
    public string VlasnikGrad { get; set; } = string.Empty;
    public DateTime DatumObjave { get; set; } = DateTime.UtcNow;
    public DateTime? DatumIzmene { get; set; }
}git