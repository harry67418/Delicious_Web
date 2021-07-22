using System;
using System.Collections.Generic;

#nullable disable

namespace prjDelicious.Models
{
    public partial class TlikeRecipe
    {
        public int RecipeLikedId { get; set; }
        public int RecipeId { get; set; }
        public int MemberId { get; set; }

        public virtual Tmember Member { get; set; }
        public virtual Trecipe Recipe { get; set; }
    }
}
