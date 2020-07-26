using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BGLOrdersAPI.Models;

namespace BGLOrdersAPI.Reports
{
    // Class used to take requests from caller to create Items.
    public class ItemReport :IBaseReport
    {
        public string ItemName { get; set; }

        public Guid? ItemId { get; set; }

        public IList<Order> Orders { get; set; }
    }
}
