using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class COrderViewModel
    {
        public Torder _order { get; set; }//一筆訂單
        public List<COrderDetailViewModel> orderdetailList { get; set; }//這筆訂單的細項
        public decimal totalPrice { get; set; }//訂單總計

        public COrderViewModel()
        {
            orderdetailList = new List<COrderDetailViewModel>();
            _order = new Torder();
        }
    }
}
