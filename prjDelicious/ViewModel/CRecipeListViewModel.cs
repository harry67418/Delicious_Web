using Microsoft.AspNetCore.Http;
using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class CRecipeListViewModel
    {
        public Trecipe recipe { get; set; }
        [DisplayName("食譜序號")]
        public int recipeId
        {
            get; set;
            //get { return recipe.RecipeId; }
            //set { recipe.RecipeId = value; }
        }
        [DisplayName("食譜名稱")]
        public string recipeName
        {
            get; set;
            //get { return recipe.RecipeName; }
            //set { recipe.RecipeName = value; }
        }
        [DisplayName("食譜描述")]
        public string recipeDescription
        {
            get; set;
            //    get { return recipe.RecipeDescription; }
            //    set { recipe.RecipeDescription = value; }
        }
        [DisplayName("點閱數")]
        public int recipeViews
        {
            get; set;
            //get { return recipe.Views; }
            //set { recipe.Views = value; }
        }
        [DisplayName("喜歡")]
        public int likes { get; set; }
        public string recipePicture { get; set; }
        public string txtSearch { get; set; }
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
