using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Management.ViewModel
{
    public class CShippermentTitleViewModel
    {
        [DisplayName("訂單編號")]
        public int orderID { get; set; }
        [DisplayName("訂單日期")]
        public string orderDate { get; set; }
        [DisplayName("會員姓名")]
        public string memberName { get; set; }
        [DisplayName("手機號碼")]
        public string cellNumber { get; set; }
        [DisplayName("付款方式")]
        public string payMethod { get; set; }
        [DisplayName("收貨方式")]
        public string recieveMethod { get; set; }
        [DisplayName("收貨地址")]
        public string address { get; set; }
        [DisplayName("總價")]
        public decimal totalprice { get; set; }
        [DisplayName("訂單狀態")]
        public string orderstatus { get; set; }
    }
}
