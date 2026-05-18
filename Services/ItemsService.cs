using lmt.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace lmt.Services;

public sealed class ItemsService : IItemsService
{
    private readonly IMongoCollection<Item> _items;

    public ItemsService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
    {
        var mongoDbSettings = settings.Value;
        var database = mongoClient.GetDatabase(mongoDbSettings.DatabaseName);
        _items = database.GetCollection<Item>(mongoDbSettings.ItemsCollectionName);
    }

    public async Task<IReadOnlyList<Item>> GetItemsAsync(CancellationToken cancellationToken)
    {
        return await _items
            .Find(FilterDefinition<Item>.Empty)
            .SortByDescending(item => item.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
