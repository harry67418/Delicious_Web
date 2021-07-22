using Microsoft.AspNetCore.Mvc;
using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.Controllers
{
    public class ApiHomePageController : Controller
    {
        DeliciousContext db = new DeliciousContext();

        public IActionResult GetRecipeCategory()
        {
            var categories = db.TrecipeCategories.Select(n => n);
            return Json(categories);
        }
        public IActionResult GetRecipeList(int id)
        {
            var recipes = (from r in db.Trecipes.AsEnumerable()
                     let like = (from r2 in db.TlikeRecipes.AsEnumerable() where r2.RecipeId == r.RecipeId select r2).Count()
                     select new
                     {
                         r.RecipeId,
                         r.RecipeName,
                         r.Member.Nickname,
                         r.RecipeDescription,
                         r.Picture,
                         r.Views,
                         like = like
                     });
            return Json(recipes);
        }
        public IActionResult GetHotSearch()
        {
            var hotsearch = ((from r in db.Trecipes
                      where r.DisVisible == false && r.DeleteOrNot == false
                      orderby r.Views descending
                      select r.RecipeName).Take(10));
            return Json(hotsearch);
        }
    }
}
