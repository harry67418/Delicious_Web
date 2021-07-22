using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Management.ViewModel
{
    public class COrderViewModel
    {
        [DisplayName("訂單日期")]
        public string OrderDate { get; set; }

        [DisplayName("訂單編號")]
        public int OrderId { get; set; }
        [DisplayName("會員編號")]
        public int MemberId { get; set; }
        
        [DisplayName("宅配縣市")]
        public string DeliveryCounty { get; set; }
        [DisplayName("付款方式")]
        public string PayMethod { get; set; }
        
        [DisplayName("消費金額")]
        public int TotalPrice { get; set; }
       
        [DisplayName("出貨日期")]
        public string DeliveredDate { get; set; }
        [DisplayName("訂單狀態")]
        public string OrderStatus { get; set; }
       

        
    }
}
