using System;
using System.Collections.Generic;

#nullable disable

namespace prjDelicious.Models
{
    public partial class Tstep
    {
        public int StepsId { get; set; }
        public int RecipeId { get; set; }
        public int StepsNumber { get; set; }
        public string Steps { get; set; }
        public string Picture { get; set; }

        public virtual Trecipe Recipe { get; set; }
    }
}
