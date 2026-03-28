using NoName.Domain.Enums;
using System;
using System.Collections.Generic;

namespace NoName.Shared.DTOs.Orders
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipEmail { get; set; }
        public string ShipPhoneNumber { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderDetailDto> Details { get; set; } = new();
    }
}
