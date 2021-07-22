using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjDelicious.Models;

namespace prjDelicious.Controllers
{
    public class MarketController : Controller
    {
        private readonly DeliciousContext _context;

        public MarketController(DeliciousContext context)
        {
            _context = context;
        }

        // GET: Market
        public async Task<IActionResult> Index()
        {
            var deliciousContext = _context.Tingredients.Where(t=>t.InStoreOrNot==true).Include(t => t.IngredientCategory);
            return View(await deliciousContext.ToListAsync());
        }

        // GET: Market/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tingredient = await _context.Tingredients
                .Include(t => t.IngredientCategory)
                .FirstOrDefaultAsync(m => m.IngredientId == id);
            if (tingredient == null)
            {
                return NotFound();
            }

            return View(tingredient);
        }
       
        

        private bool TingredientExists(int id)
        {
            return _context.Tingredients.Any(e => e.IngredientId == id);
        }
    }
}
