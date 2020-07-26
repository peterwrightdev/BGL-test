using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BGLOrdersAPI.DataContexts;
using BGLOrdersAPI.Models;
using BGLOrdersAPI.Services;

namespace BGLOrdersAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private BGLContext _context;

        // private IService _service;

        private readonly ILogger<OrdersController> _logger;

        // Use dependency injection to allow mocking of datetime used at runtime.
        private DateTimeService _dateTimeService;

        public OrdersController(ILogger<OrdersController> logger, BGLContext bglContext, DateTimeService dateTimeService)
        {
            this._logger = logger;
            this._context = bglContext;
            this._dateTimeService = dateTimeService;
            // this._service = service;
        }

        // GET: /Orders
        [HttpGet]
        public async Task<object> GetOrders()
        {
            // Use linq to retrieve all the orders and their associated items.
            var result = from order in _context.Orders
            join itemorder in _context.ItemOrderMaps on new { OrderId = order.OrderId ?? new Guid() } equals new { OrderId = itemorder.OrderId}
            join item in _context.Items on new { ItemId = itemorder.ItemId} equals new { ItemId = item.ItemId ?? new Guid() } into itemgroup
            from iog in itemgroup.DefaultIfEmpty()
            select new
            {
                OrderId = order.OrderId,
                OrderName = order.OrderName,
                AddedDateTime = order.AddedDateTime,
                Items = iog,
                LastUpdatedDateTime = order.LastUpdatedDateTime,
            };

            return await result.ToListAsync();
        }

        // GET: /Orders/4636e7fc-f4c6-4ea3-bdca-08ece857c136
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }
    }
}
