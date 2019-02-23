using System;

namespace OrdersService.Messages
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
    }
}