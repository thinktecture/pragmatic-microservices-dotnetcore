using System;
using System.Collections.Generic;

namespace OrdersService.Messages
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}