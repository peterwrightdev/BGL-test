using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BGLOrdersAPI.DataContexts;
using BGLOrdersAPI.Models;

namespace BGLOrdersAPI.Pages
{
    public class OrderModel : PageModel
    {
        private readonly BGLOrdersAPI.DataContexts.BGLContext _context;

        public OrderModel(BGLOrdersAPI.DataContexts.BGLContext context)
        {
            _context = context;
        }


        [BindProperty]
        public Order Order { get; set; }

        [BindProperty]
        public List<Order> Orders { get; set; }

        public async Task OnGetAsync()
        {
            this.Orders = await _context.Orders.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Orders.Add(Order);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
