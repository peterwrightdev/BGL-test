using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BGLOrdersAPI.Models;

namespace BGLOrdersAPI.DataContexts
{

    public class BGLContext : DbContext
    {
        public BGLContext()
            : base()
        {
        }

        public BGLContext(DbContextOptions<BGLContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemOrderMap>().HasKey(iom => new { iom.OrderId, iom.ItemId });
        }

        public DbSet<Item> Items { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<ItemOrderMap> ItemOrderMaps { get; set;}
    }
}
