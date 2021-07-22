using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Controllers
{
   
    public class ManagementChatController : Controller
    {
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
    }
}
