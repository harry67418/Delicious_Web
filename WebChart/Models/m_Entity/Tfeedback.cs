using System;
using System.Collections.Generic;

#nullable disable

namespace WebChart.Models
{
    public partial class Tfeedback
    {
        public int FeedbackId { get; set; }
        public int FeedbackCategoryId { get; set; }
        public int MemberId { get; set; }
        public string Contents { get; set; }
        public int ProgressId { get; set; }
        public int? Comment { get; set; }
        public DateTime FeedbackDate { get; set; }
        public string FeedbackContent { get; set; }
        public string Picture { get; set; }

        public virtual TfeedbackCategory FeedbackCategory { get; set; }
        public virtual Tmember Member { get; set; }
        public virtual TfeedbackProgress Progress { get; set; }
    }
}
