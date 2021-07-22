using System;
using System.Collections.Generic;

#nullable disable

namespace prjDelicious.Models
{
    public partial class TfeedbackCategory
    {
        public TfeedbackCategory()
        {
            Tfeedbacks = new HashSet<Tfeedback>();
        }

        public int FeedbackCategoryId { get; set; }
        public string FeedbackCategory { get; set; }

        public virtual ICollection<Tfeedback> Tfeedbacks { get; set; }
    }
}
