using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class CSearchRecipeViewModel
    {
        public Trecipe recipe { get; set; }
        [DisplayName("")]
        public int recipeId
        {
            get { return recipe.RecipeId; }
            set { recipe.RecipeId = value; }
        }
        [DisplayName("食譜名稱")]
        public string recipeName
        {
            get { return recipe.RecipeName; }
            set { recipe.RecipeName = value; }
        }
        //public string recipeDescription
        //{
        //    get { return recipe.RecipeDescription; }
        //    set { recipe.RecipeDescription = value; }
        //}
        //public int recipeViews
        //{
        //    get { return recipe.Views; }
        //    set { recipe.Views = value; }
        //}
        //public string recipePicture
        //{
        //    get { return recipe.Picture; }
        //    set { recipe.Picture = value; }
        //}
        //public int recipeCategoryId
        //{
        //    get { return recipe.RecipeCategoryId; }
        //    set { recipe.RecipeCategoryId = value; }
        //}
    }
}
