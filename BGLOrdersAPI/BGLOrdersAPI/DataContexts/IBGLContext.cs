using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BGLOrdersAPI.Models;

namespace BGLOrdersAPI.DataContexts
{
    public interface IBGLContext
    {
        DbSet<Item> Items { get; set; }

        DbSet<Order> Orders { get; set; }

        DbSet<ItemOrderMap> ItemOrderMaps { get; set; }
    }
}
