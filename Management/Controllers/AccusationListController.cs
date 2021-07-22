using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Management.Models;
using Management.ViewModel;

namespace WebChart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccusationListController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly DeliciousContext _context;
        public AccusationListController(DeliciousContext context, IWebHostEnvironment webHostEnvironment)
        {
            this._webHostEnvironment = webHostEnvironment;
            this._context = context;
        }
        public IActionResult AccusationList()
        {
            var cities = _context.TaccuseContents.Select(a => new { a.Accusation });

            return Json(cities);
        }
        
    }
}
