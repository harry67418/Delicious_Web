using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class CWishRecipeViewModel
    {
        public Trecipe trecipe { get; set; } //食譜
        public string _recipecategorie { get; set; } //食譜類別
        public List<CIngredientViewModel> _ingredientslist { get; set; }        
        
        public CWishRecipeViewModel()
        {
            _ingredientslist = new List<CIngredientViewModel>();
            trecipe = new Trecipe();
        }
    }
}
