using OrderInventory.Application.Orders.Dto;
using System;

namespace OrderInventory.Application.Orders.CreateOrder;

public record CreateOrderCommand(Guid customerId, List<OrderProductLineDto> Lines);
