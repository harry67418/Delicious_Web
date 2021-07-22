using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.ViewModel
{
    public class CEditOrderStatusViewModel
    {
        public DateTime deliveredDate { get; set; }
        public string orderStatus { get; set; }
        public int orderID { get; set; }
    }
}
