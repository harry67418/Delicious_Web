using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using prjDelicious.Models;
using prjDelicious.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.Controllers
{
    public class AddingCartAPIController : Controller
    {
        private readonly DeliciousContext _deliciousContext;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public AddingCartAPIController(IWebHostEnvironment hostingEnvironment, DeliciousContext deliciousContext)
        {
            _deliciousContext = deliciousContext;
            _hostingEnvironment = hostingEnvironment;
        }
        public List<CCartItemViewModel> AddtoCart(int ingid, int qty)
        {
            string cartstring = "";
            List<CCartItemViewModel> Cartlist = new List<CCartItemViewModel>();
            CCartItemViewModel cCart = null;
            if (ingid == 0 || qty == 0) 
            {
                if (HttpContext.Session.GetString("Cart") != null)
                {
                    cartstring = HttpContext.Session.GetString("Cart");
                    Cartlist = JsonConvert.DeserializeObject<List<CCartItemViewModel>>(cartstring);
                    Cartlist = Addtoiteminfo(Cartlist);
                    return Cartlist;
                }
                else
                {
                    return null;
                }
            }
           
            if (HttpContext.Session.GetString("Cart") != null)
            {
                cartstring = HttpContext.Session.GetString("Cart");
                Cartlist = JsonConvert.DeserializeObject<List<CCartItemViewModel>>(cartstring);
                if (Cartlist.Where(n => n.Ingid == ingid).Count() > 0)
                {
                    int x = Cartlist.Where(n => n.Ingid == ingid).Select(n => n.CartQty).FirstOrDefault();
                    Cartlist.Remove(Cartlist.Where(n => n.Ingid == ingid).First());
                    cCart = new CCartItemViewModel { Ingid = ingid, CartQty = (x + qty) };
                    Cartlist.Add(cCart);

                }
                else
                {
                    cCart = new CCartItemViewModel { Ingid = ingid, CartQty = qty };
                    Cartlist.Add(cCart);
                }

            }
            else
            {
                cCart = new CCartItemViewModel { Ingid = ingid, CartQty = qty };
                Cartlist.Add(cCart);
            }
            Cartlist = Addtoiteminfo(Cartlist);


            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(Cartlist));
            return Cartlist;
        }
        public List<CCartItemViewModel> Addtoiteminfo(List<CCartItemViewModel> cartlist)
        {
            List<CCartItemViewModel> Cartlist = new List<CCartItemViewModel>();
            Cartlist = cartlist;
            foreach (var item in Cartlist)
            {
                item.ingredient = _deliciousContext.Tingredients.Where(n => n.IngredientId == item.Ingid).Select(n => n.Ingredient).FirstOrDefault();
            }
            foreach (var item in Cartlist)
            {
                item.unitprice = _deliciousContext.Tingredients.Where(n => n.IngredientId == item.Ingid).Select(n => n.Price).FirstOrDefault();
            }
            foreach (var item in Cartlist)
            {
                item.amountInStore = _deliciousContext.Tingredients.Where(n => n.IngredientId == item.Ingid).Select(n => n.AmountInStore).FirstOrDefault();
            }
            
            return Cartlist;

        }
        public string RemovetoCart(int ingid)
        {
            string cartstring = "";
            List<CCartItemViewModel> Cartlist = new List<CCartItemViewModel>();
            CCartItemViewModel cCart = null;
            if (HttpContext.Session.GetString("Cart") != null)
            {
                cartstring = HttpContext.Session.GetString("Cart");
                Cartlist = JsonConvert.DeserializeObject<List<CCartItemViewModel>>(cartstring);
                if (Cartlist.Where(n => n.Ingid == ingid).Count() > 0)
                {
                    int x = Cartlist.Where(n => n.Ingid == ingid).Select(n => n.CartQty).FirstOrDefault();
                    Cartlist.Remove(Cartlist.Where(n => n.Ingid == ingid).First());
                    Cartlist = Addtoiteminfo(Cartlist);
                    HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(Cartlist));

                }
                

            }
           

            return "移除成功";
        }
        public string ChangetoCart(int ingid,int qty)
        {
            string cartstring = "";
            List<CCartItemViewModel> Cartlist = new List<CCartItemViewModel>();
            CCartItemViewModel cCart = null;
            if (HttpContext.Session.GetString("Cart") != null)
            {
                cartstring = HttpContext.Session.GetString("Cart");
                Cartlist = JsonConvert.DeserializeObject<List<CCartItemViewModel>>(cartstring);
                if (Cartlist.Where(n => n.Ingid == ingid).Count() > 0)
                {
                    
                    Cartlist.Remove(Cartlist.Where(n => n.Ingid == ingid).First());
                    cCart = new CCartItemViewModel { Ingid = ingid, CartQty = qty };
                    Cartlist.Add(cCart);

                }
               

            }
            Cartlist = Addtoiteminfo(Cartlist);
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(Cartlist));
            return "修改成功";
        }

    }
}
