using Management.Models;
using Management.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Controllers
{
    [Authorize]
    public class BusinessWebsiteController : Controller
    {
        private readonly DeliciousContext _deliciousContext;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public BusinessWebsiteController(IWebHostEnvironment hostingEnvironment, DeliciousContext deliciousContext)
        {
            _deliciousContext = deliciousContext;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            double a, b, c;
            
            ViewBag.membercount = _deliciousContext.Tmembers.Count();
            a = _deliciousContext.Tingredients.Where(n => n.InStoreOrNot).Count();
            ViewBag.merchadisecount = a;
            ViewBag.countofdealingorder = _deliciousContext.Torders.Where(n=>n.OrderStatus.Equals("已付款")).Count();
            ViewBag.countofdeliveringorder = _deliciousContext.Torders.Where(n => n.OrderStatus.Equals("出貨中")).Count();
            b = _deliciousContext.Tingredients.Where(n => n.AmountInStore <= 50 && n.InStoreOrNot).Count();
            ViewBag.lowquantity = b;
            c = _deliciousContext.Tingredients.Where(n => n.AmountInStore >= 120 && n.InStoreOrNot).Count();
            ViewBag.highquantity = c;
            ViewBag.percentlow = $"{ ((b / a) * 100)}" +"%";
            ViewBag.percenthigh = $"{ ((c / a) * 100)}" +"%";



            return View();
        }

        public IActionResult MerchadiseRank()
        {

            CBusinessWebsiteViewModel cBusinessWebsiteViewModel = new CBusinessWebsiteViewModel(); 
            
            
            var thismonthsum = _deliciousContext.TorderDetails.Where(n => DateTime.Compare(n.Order.OrderDate, DateTime.Now.AddDays(-30)) >= 0).Where(n => n.Order.OrderStatus == "已送達").GroupBy(n => n.IngredientId).OrderByDescending(n => n.Sum(m => m.InCartQuantity)).Select(n => new
            {
                IngredientId = n.Key,
                thismonthsum = n.Sum(n => n.InCartQuantity)

            }).Take(10);
           
            
           
            foreach (var item in thismonthsum)
            {
                CBusinessWebsiteItemViewModel cBusinessWebsiteItemViewModel = new CBusinessWebsiteItemViewModel() { 
                ingredientId=item.IngredientId,
                thismonthsum=item.thismonthsum
                };
             cBusinessWebsiteViewModel._MerchadiseRanklist.Add(cBusinessWebsiteItemViewModel);
            }

            foreach (var item in cBusinessWebsiteViewModel._MerchadiseRanklist)
            {
                var lastmonthsum = _deliciousContext.TorderDetails.Where(n =>(n.IngredientId==item.ingredientId)&&((DateTime.Compare(n.Order.OrderDate, DateTime.Now.AddDays(-30)) )<= 0) && ((DateTime.Compare( DateTime.Now.AddDays(-60), n.Order.OrderDate)) >= 0)&&n.Order.OrderStatus=="已送達").Sum(n=>n.InCartQuantity);

                if (lastmonthsum != 0)
                { item.lastmonthsum = lastmonthsum; }
                else
                { item.lastmonthsum = 0; }
                var ingredientinfo = _deliciousContext.Tingredients.Single(n => n.IngredientId == item.ingredientId);
                item.ingredient = ingredientinfo.Ingredient;
                item.quantity = ingredientinfo.AmountInStore;


            }


            return Json(cBusinessWebsiteViewModel);
        }

    }
}
