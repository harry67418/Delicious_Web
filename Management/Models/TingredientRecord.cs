using System;
using System.Collections.Generic;

#nullable disable

namespace Management.Models
{
    public partial class TingredientRecord
    {
        public int IngredientRecordId { get; set; }
        public int IngredientId { get; set; }
        public int RecipeId { get; set; }
        public string Unt { get; set; }
        public double IngredientAmountForUse { get; set; }

        public virtual Tingredient Ingredient { get; set; }
        public virtual Trecipe Recipe { get; set; }
    }
}
