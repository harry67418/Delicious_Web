using System;
using System.Collections.Generic;

#nullable disable

namespace WebChart.Models
{
    public partial class TingredientCategory
    {
        public TingredientCategory()
        {
            Tingredients = new HashSet<Tingredient>();
        }

        public int IngredientCategoryId { get; set; }
        public string IngredientCategory { get; set; }

        public virtual ICollection<Tingredient> Tingredients { get; set; }
    }
}
