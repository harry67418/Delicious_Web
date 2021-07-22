using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Management.Models;
 

namespace Management.ViewModel
{
    public class CMemberFeedbackcs
    {
        public int FeedbackId { get; set; }
        public string FeedbackCategory { get; set; }
        public int MemberId { get; set; }
        public string Contents { get; set; }
        public string ProgressContent { get; set; }
        public int? Comment { get; set; }
        public DateTime FeedbackDate { get; set; }
        public string FeedbackContent { get; set; }
        public string Picture { get; set; }

       

    }
}
