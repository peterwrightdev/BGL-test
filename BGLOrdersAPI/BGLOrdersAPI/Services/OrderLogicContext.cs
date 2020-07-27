using BGLOrdersAPI.DataContexts;
using BGLOrdersAPI.Models;
using BGLOrdersAPI.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BGLOrdersAPI.Services
{
    public class OrderLogicContext : ILogicContext<Order, OrderReport>
    {
        private BGLContext _context;
        private DateTimeService _dateTimeService;

        // Injection so that this can be unit tested
        public OrderLogicContext(BGLContext context, DateTimeService dateTimeService)
        {
            this._context = context;
            this._dateTimeService = dateTimeService;
        }

        public Order ConvertReportToModel(OrderReport orderReport)
        {
            Order order = new Order()
            {
                OrderId = orderReport.OrderId,
                OrderName = orderReport.OrderName,
                LastUpdatedDateTime = this._dateTimeService.UtcDateTime(),
            };

            if (orderReport.Items != null)
            {
                order.ItemOrderMaps = new List<ItemOrderMap>();
                foreach (Item item in orderReport.Items)
                {
                    order.ItemOrderMaps.Add(new ItemOrderMap() { Item = item, Order = order });
                }
            }

            // if creating a new record, set the current datetime
            if (orderReport.OrderId == null)
            {
                order.AddedDateTime = this._dateTimeService.UtcDateTime();
            }

            return order;
        }

        public bool Validate(Order order)
        {
            // In reality, we'd want to return useful error messages in case of validation failure. Just returning false here for speed of dev.
            bool passesValidation = true;

            // If Item has an id, verify that a record of the ID already exists
            if (order.OrderId != null && this._context.Items.Find(order.OrderId) == null)
            {
                passesValidation = false;
            }

            // for each order being referenced, verify that any that supposedly currently exist do in fact exist
            if (order.ItemOrderMaps != null)
            {
                var items = order.ItemOrderMaps.Select(iom => iom.Item).ToList();
                foreach (Item item in items)
                {
                    if (item.ItemId != null && this._context.Orders.Find(item.ItemId) == null)
                    {
                        passesValidation = false;
                    }
                }
            }

            return passesValidation;
        }
    }
}
