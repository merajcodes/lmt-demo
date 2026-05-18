using lmt.Models;

namespace lmt.Services;

public interface IItemsService
{
    Task<IReadOnlyList<Item>> GetItemsAsync(CancellationToken cancellationToken);
}
