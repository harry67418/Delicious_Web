using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class CFolderPageViewModel
    {
        public List<TcollectionFolder> folders { get; set; }
        public List<Trecipe> recipes { get; set; }
        public CFolderPageViewModel()
        {
            folders = new List<TcollectionFolder>();
            recipes = new List<Trecipe>();
        }
    }
}
