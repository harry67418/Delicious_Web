using System;
using System.Collections.Generic;

#nullable disable

namespace prjDelicious.Models
{
    public partial class TwishList
    {
        public int WishListId { get; set; }
        public int RecipeId { get; set; }
        public int MemberId { get; set; }
        public bool BoughtOrNot { get; set; }

        public virtual Tmember Member { get; set; }
        public virtual Trecipe Recipe { get; set; }
    }
}
