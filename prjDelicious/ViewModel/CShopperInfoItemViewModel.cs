using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class CShopperInfoItemViewModel
    {
        public CShopperInfoItemViewModel() {
            PicList = new List<string>();
            Relativelist = new List<CCartItemViewModel>();
        }
        public int IngredientId { get; set; }
        public string Ingredient { get; set; }
        public int AmountInStore { get; set; }
        public string MerchandiseDescription { get; set; }
        public decimal Price { get; set; }
        public string IngredientUnit { get; set; }
        public string Category { get; set; }
        public int CategoryID { get; set; }
        public List<string> PicList { get; set; }

        public List<CCartItemViewModel> Relativelist { get; set; }
    }
}
