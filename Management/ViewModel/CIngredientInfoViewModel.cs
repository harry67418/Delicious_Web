using Management.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.ViewModel
{
    public class CIngredientInfoViewModel
    {

        public CIngredientInfoViewModel()
        {
            CatIdList = new List<CIngredientCategoryViewModel>();
            IdList = new List<CIngredientItemViewModel>();
        }


       
        public List<CIngredientCategoryViewModel> CatIdList{ get; set; }
        public List<CIngredientItemViewModel> IdList { get; set; }


        //public virtual TingredientCategory IngredientCategory { get; set; }
    }
}
