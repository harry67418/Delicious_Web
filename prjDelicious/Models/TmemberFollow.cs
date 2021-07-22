using System;
using System.Collections.Generic;

#nullable disable

namespace prjDelicious.Models
{
    public partial class TmemberFollow
    {
        public int FollowId { get; set; }
        public int MemberId { get; set; }
        public int FollowMemberId { get; set; }

        public virtual Tmember FollowMember { get; set; }
        public virtual Tmember Member { get; set; }
    }
}
