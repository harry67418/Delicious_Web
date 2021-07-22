using System;
using System.Collections.Generic;

#nullable disable

namespace prjDelicious.Models
{
    public partial class TlikeComment
    {
        public int CommentLikedId { get; set; }
        public int CommentId { get; set; }
        public int MemberId { get; set; }

        public virtual TcommentSection Comment { get; set; }
        public virtual Tmember Member { get; set; }
    }
}
