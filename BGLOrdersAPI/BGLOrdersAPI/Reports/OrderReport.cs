using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BGLOrdersAPI.Models;

namespace BGLOrdersAPI.Reports
{
    // Class used to take requests from caller to create Orders.
    public class OrderReport : IBaseReport
    {
        public string OrderName { get; set; }

        public Guid? OrderId { get; set; }

        public IList<Item> Items { get; set; }
    }
}
