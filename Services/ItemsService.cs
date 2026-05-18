using lmt.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SeliseBlocks.LMT.Client;
using System.Diagnostics;

namespace lmt.Services;

public sealed class ItemsService : IItemsService
{
    private readonly IMongoCollection<Item> _items;
    private readonly IBlocksLogger _logger;
    private readonly ActivitySource _activitySource;

    public ItemsService(
        IMongoClient mongoClient, 
        IOptions<MongoDbSettings> settings,
        IBlocksLogger logger, 
        ActivitySource activitySource)
    {
        var mongoDbSettings = settings.Value;
        var database = mongoClient.GetDatabase(mongoDbSettings.DatabaseName);
        _items = database.GetCollection<Item>(mongoDbSettings.ItemsCollectionName);
        _logger = logger;
        _activitySource = activitySource;
    }

    public async Task<IReadOnlyList<Item>> GetItemsAsync(CancellationToken cancellationToken)
    {
        using var activity = _activitySource.StartActivity("LogTest");
        _logger.LogInformation("LogTest method call");

        return await _items
            .Find(FilterDefinition<Item>.Empty)
            .SortByDescending(item => item.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
