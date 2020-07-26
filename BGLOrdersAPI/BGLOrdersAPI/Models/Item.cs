using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BGLOrdersAPI.Models
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? ItemId { get; set; }

        public string ItemName { get; set; }

        // Note these fields should be visible to callers but cannot be modified by them
        public DateTime AddedDateTime { get; set; }
        public DateTime? LastUpdatedDateTime { get; set; }

        public IList<ItemOrderMap> ItemOrderMaps { get; set; }
    }
}
