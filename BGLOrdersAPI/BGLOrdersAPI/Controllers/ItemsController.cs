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
using BGLOrdersAPI.Reports;

namespace BGLOrdersAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private IBGLContext _context;

        private ILogicContext<Item, ItemReport> _logicContext;

        private readonly ILogger<ItemsController> _logger;

        // Use dependency injection to allow mocking of datetime used at runtime.
        private DateTimeService _dateTimeService;

        public ItemsController(ILogger<ItemsController> logger, IBGLContext bglContext, DateTimeService dateTimeService, ILogicContext<Item, ItemReport> logicContext)
        {
            this._logger = logger;
            this._context = bglContext;
            this._dateTimeService = dateTimeService;
            this._logicContext = logicContext;
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
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        [HttpPost]
        public async Task<ActionResult<Item>> CreateItem(ItemReport report)
        {
            // For simplicity, drop the id if supplied on the create action.
            report.ItemId = null;

            Item newItem = this._logicContext.ConvertReportToModel(report);
            if (this._logicContext.Validate(newItem))
            {
                ((DbContext)this._context).Add(newItem);
                await ((DbContext)this._context).SaveChangesAsync();

                return CreatedAtAction("GetItems", new { id = newItem.ItemId }, newItem);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<ActionResult<Item>> UpdateItem(ItemReport report)
        {
            // For simplicity, drop the id if supplied on the create action.
            Item existingItem = this._logicContext.ConvertReportToModel(report);
            if (this._logicContext.Validate(existingItem))
            {
                ((DbContext)this._context).Update(existingItem);
                await ((DbContext)this._context).SaveChangesAsync();

                return CreatedAtAction("GetItems", new { id = existingItem.ItemId }, existingItem);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
