using System;
using System.Collections.Generic;

#nullable disable

namespace Management.Models
{
    public partial class TfeedbackProgress
    {
        public TfeedbackProgress()
        {
            Tfeedbacks = new HashSet<Tfeedback>();
        }

        public int ProgressId { get; set; }
        public string ProgressContent { get; set; }

        public virtual ICollection<Tfeedback> Tfeedbacks { get; set; }
    }
}
