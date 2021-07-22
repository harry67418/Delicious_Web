using System;
using System.Collections.Generic;

#nullable disable

namespace WebChart.Models
{
    public partial class Taccusation
    {
        public int AccusationRightId { get; set; }
        public int MemberId { get; set; }
        public int AccuseId { get; set; }
        public string AccusedAvatar { get; set; }
        public string AccusedId { get; set; }
        public int ProgressId { get; set; }

        public virtual TaccuseContent Accuse { get; set; }
        public virtual Tmember Member { get; set; }
    }
}
