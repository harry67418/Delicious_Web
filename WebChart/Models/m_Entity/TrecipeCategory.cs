using System;
using System.Collections.Generic;

#nullable disable

namespace WebChart.Models
{
    public partial class TrecipeCategory
    {
        public TrecipeCategory()
        {
            Trecipes = new HashSet<Trecipe>();
        }

        public int RecipeCategoryId { get; set; }
        public string RecipeCategory { get; set; }

        public virtual ICollection<Trecipe> Trecipes { get; set; }
    }
}
