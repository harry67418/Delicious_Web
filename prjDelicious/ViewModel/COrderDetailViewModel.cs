using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class COrderDetailViewModel
    {        
        public TorderDetail _orderdetail { get; set; }//細項的table    
        public string Ingredient { get; set; } //食材名稱
        public string Unit { get; set; } //食材單位
        public COrderDetailViewModel()
        {
            _orderdetail = new TorderDetail();
        }
    }
}
