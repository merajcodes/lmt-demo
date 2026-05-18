using lmt.Models;
using lmt.Services;
using Microsoft.AspNetCore.Mvc;

namespace lmt.Controllers;

[ApiController]
[Route("api/items")]
public sealed class ItemsController : ControllerBase
{
    private readonly IItemsService _itemsService;

    public ItemsController(IItemsService itemsService)
    {
        _itemsService = itemsService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Item>>> GetItems(CancellationToken cancellationToken)
    {
        var items = await _itemsService.GetItemsAsync(cancellationToken);
        return Ok(items);
    }
}
