using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class CCartItemViewModel
    {
      
        
         public int Ingid { get; set; }
         public int CartQty { get; set; }
         public string ingredient { get; set; }
         public decimal unitprice { get; set; }
         public int amountInStore { get; set; }
        public string ingpicsrc { get; set; }

    }

   
}

