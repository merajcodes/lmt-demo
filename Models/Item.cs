using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace lmt.Models;

public sealed class Item
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; init; }

    [BsonElement("name")]
    public string Name { get; init; } = string.Empty;

    [BsonElement("description")]
    public string? Description { get; init; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; init; }
}
