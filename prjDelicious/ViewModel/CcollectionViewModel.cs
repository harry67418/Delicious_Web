using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class CCollectionViewModel
    {
        public Tcollection collection { get; set; }
        public List<Trecipe> recipres { get; set; }
        public CCollectionViewModel()
        {
            collection = new Tcollection();
            recipres = new List<Trecipe>();
        }
    }
}
