using OrderInventory.Application.Orders.Dto;
using OrderInventory.Domain.Orders;
using System;

namespace OrderInventory.Application.Orders.CreateOrder;

public record CreateOrderResult(Guid Id, OrderStatus status, IReadOnlyCollection<OrderProductLineDto> Lines);

