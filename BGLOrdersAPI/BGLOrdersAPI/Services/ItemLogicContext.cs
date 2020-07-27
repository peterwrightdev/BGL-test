using BGLOrdersAPI.DataContexts;
using BGLOrdersAPI.Models;
using BGLOrdersAPI.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BGLOrdersAPI.Services
{
    public class ItemLogicContext : ILogicContext<Item, ItemReport>
    {
        private IBGLContext _context;
        private DateTimeService _dateTimeService;

        // Injection so that this can be unit tested
        public ItemLogicContext(IBGLContext context, DateTimeService dateTimeService)
        {
            this._context = context;
            this._dateTimeService = dateTimeService;
        }

        public Item ConvertReportToModel(ItemReport itemReport)
        {
            Item item = new Item()
            {
                ItemId = itemReport.ItemId,
                ItemName = itemReport.ItemName,
                LastUpdatedDateTime = this._dateTimeService.UtcDateTime(),
            };

            if (itemReport.Orders != null)
            {
                item.ItemOrderMaps = new List<ItemOrderMap>();
                foreach (Order order in itemReport.Orders)
                {
                    item.ItemOrderMaps.Add(new ItemOrderMap() { Item = item, Order = order });

                    if (order.OrderId == null)
                    {
                        order.AddedDateTime = this._dateTimeService.UtcDateTime();
                    }
                }
            }

            // if creating a new record, set the current datetime
            if (itemReport.ItemId == null)
            {
                item.AddedDateTime = this._dateTimeService.UtcDateTime();
            }

            return item;
        }

        public bool Validate(Item item)
        {
            // In reality, we'd want to return useful error messages in case of validation failure. Just returning false here for speed of dev.
            bool passesValidation = true;

            // If Item has an id, verify that a record of the ID already exists
            if (item.ItemId != null && this._context.Items.Find(item.ItemId) == null)
            {
                passesValidation = false;
            }

            // for each order being referenced, verify that any that supposedly currently exist do in fact exist
            if (item.ItemOrderMaps != null)
            {
                var orders = item.ItemOrderMaps.Select(iom => iom.Order).ToList();
                foreach (Order order in orders)
                {
                    if (order.OrderId != null && this._context.Orders.Find(order.OrderId) == null)
                    {
                        passesValidation = false;
                    }
                }
            }

            return passesValidation;
        }
    }
}
