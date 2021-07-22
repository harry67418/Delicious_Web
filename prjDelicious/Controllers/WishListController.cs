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
    public class WishListController : Controller
    {
        private readonly DeliciousContext _deliciousContext;
        public WishListController(DeliciousContext deliciousContext)
        {
            _deliciousContext = deliciousContext;
        }

        CWishListViewModel model = new CWishListViewModel();

        public IActionResult List(int memberid)
        {            
            var wishrecipe = _deliciousContext.TwishLists.Where(m => m.MemberId == memberid).Select(n=>new 
            {
                RecipeId=n.RecipeId,
                CategoryName=n.Recipe.RecipeCategory.RecipeCategory,
                RecipeName=n.Recipe.RecipeName, 
                Picture=n.Recipe.Picture,
            }).ToList();            
            
            foreach (var item in wishrecipe)
            {
                CWishRecipeViewModel x = new CWishRecipeViewModel();
                x.trecipe.RecipeId = item.RecipeId;
                x.trecipe.RecipeName = item.RecipeName;
                x.trecipe.Picture = item.Picture;
                x._recipecategorie = item.CategoryName;
                model.list.Add(x);               
            } 
            foreach(var item in model.list)
            {
                var q = _deliciousContext.TingredientRecords.Where(r => r.RecipeId == item.trecipe.RecipeId).Select(t =>new
                {
                    t,
                    t.Ingredient,
                });
                foreach(var r in q)
                {
                    CIngredientViewModel x = new CIngredientViewModel();
                    x._ingredient = r.Ingredient;
                    x._ingredientrecords = r.t;
                    item._ingredientslist.Add(x);
                }                
            }

            //食材與購物車比對
            foreach(var item in model.list)
            {
                foreach (var ING in item._ingredientslist)
                {
                    if(HttpContext.Session.GetString("Cart")!=null)
                    {
                        string INGstring = HttpContext.Session.GetString("Cart");
                        var INGList = JsonConvert.DeserializeObject<List<CCartItemViewModel>>(INGstring);
                        if (INGList.Where(n => n.Ingid == ING._ingredient.IngredientId).Count() > 0)
                        {
                            int cartqty = INGList.Where(n => n.Ingid == ING._ingredient.IngredientId).Select(n => n.CartQty).FirstOrDefault();
                            ING._shopping.CartQty = cartqty;
                        }
                    }
                    else
                    {
                        ING._shopping.CartQty = 0;
                    }                  
                }
            }
            return View(model);
        } 

        //刪除願望清單中的食譜
        public bool Delete(int memberid, int recipeid)
        {
            var recipedelete = _deliciousContext.TwishLists.FirstOrDefault(r => r.MemberId==memberid && r.RecipeId == recipeid);            
            _deliciousContext.Remove(recipedelete);
            try
            {
                _deliciousContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }          
        }

        //使用者將食譜加至願望清單
        public string AddIntoWish(int memberid,int recipeid)
        {//判斷使用者加入的食譜是否有重複
            int countsame = CountReapt(memberid, recipeid);

            if (countsame > 0)
            {
                return "此食譜重複新增";
            }
            else
            {
                TwishList wishlist = new TwishList();
                wishlist.MemberId = memberid;
                wishlist.RecipeId = recipeid;
                _deliciousContext.TwishLists.Add(wishlist);
                try
                {
                    _deliciousContext.SaveChanges();
                    return "已加至願望清單內";
                }
                catch
                {
                    return "新增失敗";
                }                 
            }
        }

        //判斷使用者加入的食譜是否有重複
        public int CountReapt(int memberid, int recipeid)
        {
            return _deliciousContext.TwishLists.Where(m => m.MemberId == memberid && m.RecipeId == recipeid).Count();
        }
    }
}
