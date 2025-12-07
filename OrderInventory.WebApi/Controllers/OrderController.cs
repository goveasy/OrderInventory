using Microsoft.AspNetCore.Mvc;
using OrderInventory.Application.Orders.ConfirmOrder;

namespace OrderInventory.WebApi.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController: ControllerBase
{
    private readonly ConfirmOrderService _confirmOrderService;

    public OrderController(ConfirmOrderService confirmOrderService)
    {
        _confirmOrderService = confirmOrderService;
    }

    [HttpPost("{orderId}/confirm")]
    public async Task<IActionResult> ConfirmOrderAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var command = new ConfirmOrderCommand(orderId);
        await _confirmOrderService.ConfirmOrderAsync(command, cancellationToken);
        return Ok();
    }
}
