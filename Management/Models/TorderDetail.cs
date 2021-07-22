using System;
using System.Collections.Generic;

#nullable disable

namespace Management.Models
{
    public partial class TorderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderiD { get; set; }
        public int IngredientId { get; set; }
        public decimal Price { get; set; }
        public double Discount { get; set; }
        public int InCartQuantity { get; set; }

        public virtual Tingredient Ingredient { get; set; }
        public virtual Torder Order { get; set; }
    }
}
