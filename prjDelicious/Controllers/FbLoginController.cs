using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.Controllers
{
    public class FbLoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
