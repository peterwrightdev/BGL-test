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
    public class ItemsController : ControllerBase
    {
        private BGLContext _context;

        // private IService _service;

        private readonly ILogger<ItemsController> _logger;

        // Use dependency injection to allow mocking of datetime used at runtime.
        private DateTimeService _dateTimeService;

        public ItemsController(ILogger<ItemsController> logger, BGLContext bglContext, DateTimeService dateTimeService)
        {
            this._logger = logger;
            this._context = bglContext;
            this._dateTimeService = dateTimeService;
            // this._service = service;
        }

        // GET: /Items
        [HttpGet]
        public async Task<object> GetItems()
        {
            // Use linq to retrieve all the orders and their associated items.
            var result = from item in _context.Items
                         join itemorder in _context.ItemOrderMaps on new { ItemId = item.ItemId ?? new Guid() } equals new { ItemId = itemorder.ItemId }
                         join order in _context.Orders on new { OrderId = itemorder.OrderId } equals new { OrderId = order.OrderId ?? new Guid() } into ordergroup
                         from og in ordergroup.DefaultIfEmpty()
                         select new
                         {
                             ItemId = item.ItemId,
                             ItemName = item.ItemName,
                             AddedDateTime = item.AddedDateTime,
                             Orders = og,
                             LastUpdatedDateTime = item.LastUpdatedDateTime,
                         };


            return await result.ToListAsync();
        }

        // GET: /Items/4636e7fc-f4c6-4ea3-bdca-08ece857c136
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Item>> GetItems(Guid id)
        {
            var paymentReport = await _context.Items.FindAsync(id);

            if (paymentReport == null)
            {
                return NotFound();
            }

            return paymentReport;
        }
    }
}
