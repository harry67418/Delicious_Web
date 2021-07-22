using System;
using System.Collections.Generic;

#nullable disable

namespace Management.Models
{
    public partial class TcommentSection
    {
        public TcommentSection()
        {
            TlikeComments = new HashSet<TlikeComment>();
        }

        public int CommentId { get; set; }
        public int RecipeId { get; set; }
        public int MemberId { get; set; }
        public string Comment { get; set; }
        public string Picture { get; set; }
        public string Video { get; set; }
        public DateTime PostTime { get; set; }
        public bool DisVisible { get; set; }
        public bool DeleteOrNot { get; set; }

        public virtual Tmember Member { get; set; }
        public virtual Trecipe Recipe { get; set; }
        public virtual ICollection<TlikeComment> TlikeComments { get; set; }
    }
}
