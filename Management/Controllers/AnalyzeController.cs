using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Management.Controllers
{

    public class AnalyzeController : Controller
    {
        [Authorize]
        public IActionResult Sale()
        {
            return View();
        }

        [Authorize]
        public IActionResult Members()
        {
            return View();
        }

        [Authorize]
        public IActionResult Recipe()
        {
            return View();
        }
    }
}
