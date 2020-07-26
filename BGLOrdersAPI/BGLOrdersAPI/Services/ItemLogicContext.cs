using BGLOrdersAPI.DataContexts;
using BGLOrdersAPI.Models;
using BGLOrdersAPI.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BGLOrdersAPI.Services
{
    public class ItemLogicContext : ILogicContext<Item, ItemReport>
    {
        private BGLContext _context;

        // Injection so that this can be unit tested
        public ItemLogicContext(BGLContext context)
        {
            this._context = context;
        }

        public Item ConvertReportToModel(ItemReport itemReport)
        {
            Item item = new Item()
            {
                ItemId = itemReport.ItemId,
                ItemName = itemReport.ItemName,
            };

            foreach(Order order in itemReport.Orders)
            {
                item.ItemOrderMaps.Add(new ItemOrderMap() { Item = item, Order = order });
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
            var orders = item.ItemOrderMaps.Select(iom => iom.Order).ToList();
            foreach (Order order in orders)
            {
                if (order.OrderId != null && this._context.Orders.Find(order.OrderId) == null)
                {
                    passesValidation = false;
                }
            }

            return passesValidation;
        }
    }
}
