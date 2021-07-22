using System;
using System.Collections.Generic;

#nullable disable

namespace prjDelicious.Models
{
    public partial class Torder
    {
        public Torder()
        {
            TorderDetails = new HashSet<TorderDetail>();
        }

        public int OrderId { get; set; }
        public int MemberId { get; set; }
        public string Reciever { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string DeliveryCounty { get; set; }
        public string DeliveryDistrict { get; set; }
        public string DeliveryAddress { get; set; }
        public string PayMethod { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public string OrderStatus { get; set; }
        public string RecieveMethod { get; set; }
        public string MerchantTradeNo { get; set; }

        public virtual Tmember Member { get; set; }
        public virtual ICollection<TorderDetail> TorderDetails { get; set; }
    }
}
