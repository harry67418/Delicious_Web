using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Management.ViewModel
{
    public class CConditonofOrderViewModel
    {
        public DateTime SelOrderMin { get; set; }
        public DateTime SelOrderMax { get; set; }
        public DateTime SelDelMin { get; set; }
        public DateTime SelDelMax { get; set; }
        public string SelOrderStatus { get; set; }
        
        public string SelOrderID { get; set; }
        public string SelMemberID { get; set; }
        public string SelMember { get; set; }
        public string SelPhone { get; set; }
        public string SelEmail { get; set; }
        public CancellationToken cancellationToken { get; set; }
    }
}
