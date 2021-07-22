using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Models
{

   
    public class CShippermentViewModel
    {
       
     
        [DisplayName("商品")]
        public string merchadise { get; set; }
        [DisplayName("數量")]
        public int quantity { get; set; }
        [DisplayName("單價")]
        public decimal unitprice { get; set; }
       
    
     
    }
}
