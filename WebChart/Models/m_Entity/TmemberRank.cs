using System;
using System.Collections.Generic;

#nullable disable

namespace WebChart.Models
{
    public partial class TmemberRank
    {
        public TmemberRank()
        {
            Tmembers = new HashSet<Tmember>();
        }

        public int RankId { get; set; }
        public string RankName { get; set; }
        public int? RankScore { get; set; }

        public virtual ICollection<Tmember> Tmembers { get; set; }
    }
}
