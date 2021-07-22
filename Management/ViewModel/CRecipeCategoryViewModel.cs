using Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.ViewModel
{
    public class CRecipeCategoryViewModel
    {
        public TrecipeCategory category { get; set; }
        public decimal countthiscate { get; set; }
        public string countpercent { get; set; }
        public double countpercentdob { get; set; }
    }
}
