using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.Controllers
{

    public class ApiRecipePageController : Controller
    {
        private readonly DeliciousContext _delicious;

        public ApiRecipePageController(DeliciousContext delicious)
        {
            _delicious = delicious;
        }
        public IActionResult RecipeCategoryList() //食譜分類api
        {
            var RecipeCategory = _delicious.TrecipeCategories.Select(n => new
            {
                n.RecipeCategory,
                n.RecipeCategoryId
            });

            return Json(RecipeCategory);
        }


        public IActionResult IngredientCategoryList() //食材分類api
        {
            var IngredientCategory = _delicious.TingredientCategories.Select(n => new
            {
                n.IngredientCategory,
                n.IngredientCategoryId
            });

            return Json(IngredientCategory);
        }

        public IActionResult IngredientList(int id) //食材api
        {
            var Ingredient = _delicious.Tingredients.Where(n => n.IngredientCategoryId == id).Select(n => new {
                n.Ingredient,
                n.IngredientId
            });

            return Json(Ingredient);
        }
    }
}
