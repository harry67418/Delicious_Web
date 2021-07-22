using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{    
    public class CWishListViewModel
    {
        public Tmember _members { get; set; } //會員 
        public List<CWishRecipeViewModel> list { get; set; }      

        public CWishListViewModel()
        {
            _members = new Tmember();
            list = new List<CWishRecipeViewModel>();                        
        }
    }
}
