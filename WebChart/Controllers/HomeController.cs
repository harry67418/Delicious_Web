using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebChart.Models;
using WebChart.ViewModels;

namespace WebChart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly DeliciousContext _context;

        public HomeController(ILogger<HomeController> logger, DeliciousContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            this._webHostEnvironment = webHostEnvironment;
            this._context = context;   
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GroupIndex()
        {
            return View();
        }

        public IActionResult customerGroupIndex()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(NotFound());
        }

        public IActionResult live2DDemo()
        {
            return View();
        }
    }
}
