using AutoMapper;
using EasyNetQ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using OrdersService.Hubs;
using OrdersService.Messages;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OrdersService.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private static readonly ConcurrentDictionary<Guid, DTOs.Order> Datastore;
        private readonly AppSettings _settings;
        private readonly IHubContext<OrdersHub> _orderHubContext;

        static OrdersController()
        {
            Datastore = new ConcurrentDictionary<Guid, DTOs.Order>();
        }

        public OrdersController(IOptions<AppSettings> appSettingsOptions, IHubContext<OrdersHub> ordersHubContext)
        {
            _settings = appSettingsOptions.Value;
            _orderHubContext = ordersHubContext;
        }

        [HttpGet]
        public List<DTOs.Order> GetOrders()
        {
            try
            {
                return Datastore.Values.OrderByDescending(o => o.Created).ToList();
            }
            catch (Exception e)
            {
                string message = "We could not retrieve the list of orders.";
                Log.Error(message + $" Reason: {0}", e);

                throw new OrderServiceException(message);
            }
        }

        [HttpPost]
        public ActionResult AddNewOrder([FromBody] DTOs.Order newOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderId = Guid.NewGuid();
            newOrder.Id = orderId;
            newOrder.Created = DateTime.UtcNow;

            try
            {
                Datastore.TryAdd(orderId, newOrder);
            }
            catch (Exception e)
            {
                string message = "We could not add the new order.";
                Log.Error(message + $" Reason: {0}", e);

                throw new OrderServiceException(message);
            }

            // TODO: Retry & exception handling
            using (var bus = RabbitHutch.CreateBus(_settings.RabbitMqConnectionString))
            {
                var identity = User.Identity as ClaimsIdentity;
                var subjectId = identity?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

                var message = new NewOrderMessage
                {
                    UserId = subjectId,
                    Order = Mapper.Map<Order>(newOrder)
                };

                // TODO: Exception handling
                bus.Publish(message);

                _orderHubContext.Clients.Group(message.UserId).SendAsync("orderCreated");
            }

            return Ok();
        }
    }
}