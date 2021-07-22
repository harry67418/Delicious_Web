using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.ViewModel
{
    public class CIngredientItemViewModel
    {
        public CIngredientItemViewModel()
        {
            CatIdList = new List<CIngredientCategoryViewModel>();
           
        }
        public List<CIngredientCategoryViewModel> CatIdList { get; set; }
        public int IngredientId { get; set; }
        public string Ingredient { get; set; }
        public int IngredientCategoryID { get; set; }
        public string IngredientCategory { get; set; }
        public string IngredientUnit { get; set; }
        public decimal Price { get; set; }
        public int AmountInStore { get; set; }
        public string MerchandiseDescription { get; set; }
        public bool InStoreOrNot { get; set; }
        public string MerchadisePicture { get; set; }
        public IFormFile FormfilePic { get; set; }
    }
}
