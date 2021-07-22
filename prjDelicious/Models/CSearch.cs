using prjDelicious.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.Models
{
    public class CSearch
    {
        DeliciousContext DBContext = new DeliciousContext();
        public int[] RecipeSearches(DeliciousContext DBContext, string SearchString)
        {
            //string[] Searches = SearchString.Split();
            string[] Searches = "牛肉 咖哩".Split();
            var Recipes = DBContext.Trecipes.Select(r => new
            {
                r.RecipeId,
                r.RecipeName,
                r.RecipeCategory.RecipeCategory,
                Tags = DBContext.ThashtagRecords.Where(rc => rc.RecipeId == r.RecipeId).Select(t => t.HashTag.Hasgtag).ToList(),
                Ingds = DBContext.TingredientRecords.Where(rc => rc.RecipeId == r.RecipeId).Select(t => t.Ingredient.Ingredient).ToList(),
            }).ToList();
            foreach (string s in Searches)
            {
                Recipes = Recipes.Where(r => r.RecipeName.Contains(s) || r.RecipeCategory.Contains(s) || r.Tags.Any(t => t.Contains(s)) || r.Ingds.Any(i => i.Contains(s))).ToList();
            }
            return Recipes.Select(r => r.RecipeId).ToArray();
        }
        public int[] MemberSearches(DeliciousContext DBContext, string SearchString)
        {
            string[] Searches = SearchString.Split();
            var Members = DBContext.Tmembers.Select(m => new
            {
                m.MemberId,
                m.Nickname
            }).ToList();
            foreach (string s in Searches)
            {
                Members = Members.Where(m => m.Nickname.Contains(s)).ToList();
            }
            return Members.Select(m => m.MemberId).ToArray();
        }
        public IEnumerable<CSearchRecipeViewModel> SearchRecipeVM(DeliciousContext DBContext, int[] RecipeIds)
        {
            var recipes = DBContext.Trecipes.Where(r => RecipeIds.Contains(r.RecipeId)).Select(r => new
            {
                r.RecipeId,
                r.RecipeName,
                r.RecipeDescription,
                r.Views,
                r.Picture
            }).ToList();
            return recipes.Select(r => new CSearchRecipeViewModel
            {
                recipeId = r.RecipeId,
                recipeName = r.RecipeName,
                //recipeDescription = r.RecipeDescription,
                //recipeViews = r.Views,
                //recipePicture = r.Picture,
            }).AsEnumerable();
        }
    }
}
