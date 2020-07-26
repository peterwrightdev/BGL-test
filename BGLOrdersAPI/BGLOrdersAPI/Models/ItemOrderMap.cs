using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BGLOrdersAPI.Models
{
    public class ItemOrderMap
    {
        public Guid OrderId { get; set; }

        public Order Order { get; set; }

        public Guid ItemId { get; set; }

        public Item Item { get; set; }
    }
}
