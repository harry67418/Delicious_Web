using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class CIngredientViewModel
    {
        public Tingredient _ingredient { get; set; } //食材
        public TingredientRecord _ingredientrecords { get; set; } //食材使用量
        public CCartItemViewModel _shopping { get; set; } //購物車


        public CIngredientViewModel()
        {
            _ingredient = new Tingredient();
            _ingredientrecords = new TingredientRecord();
            _shopping = new CCartItemViewModel();
        }
    }
}
