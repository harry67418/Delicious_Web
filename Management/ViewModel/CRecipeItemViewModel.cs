using Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.ViewModel
{
    public class CRecipeItemViewModel
    {
        public Trecipe trecipe { get; set; }
        public TrecipeCategory trecipeCategory { get; set; }
        public int countlike { get; set; }
        public int countcomment { get; set; }
    }
}
