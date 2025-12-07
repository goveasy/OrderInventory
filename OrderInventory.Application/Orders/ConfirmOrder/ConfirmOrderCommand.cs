using System;

namespace OrderInventory.Application.Orders.ConfirmOrder;

public record ConfirmOrderCommand(Guid OrderId);

