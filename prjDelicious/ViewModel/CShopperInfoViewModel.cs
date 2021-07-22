using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class CShopperInfoViewModel
    {
        public List<CShopperInfoItemViewModel> ListofMerchadises { get; set; }
        public CShopperInfoViewModel() 
        {
            ListofMerchadises = new List<CShopperInfoItemViewModel>();
        }
    }
}
