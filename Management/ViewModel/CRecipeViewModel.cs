using Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.ViewModel
{
    public class CRecipeViewModel
    {
        public CRecipeViewModel() {
            list = new List<CRecipeItemViewModel>();
            caterecipelist = new List<CRecipeCategoryViewModel>();
        }
        public List<CRecipeItemViewModel> list { get; set; }
        public List<CRecipeCategoryViewModel> caterecipelist { get; set; }
        public decimal total { get; set; }
    }
}
