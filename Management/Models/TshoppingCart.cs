using System;
using System.Collections.Generic;

#nullable disable

namespace Management.Models
{
    public partial class TshoppingCart
    {
        public int CartId { get; set; }
        public int MemberId { get; set; }
        public int IngredientId { get; set; }
        public int InCartQuantity { get; set; }

        public virtual Tingredient Ingredient { get; set; }
        public virtual Tmember Member { get; set; }
    }
}
