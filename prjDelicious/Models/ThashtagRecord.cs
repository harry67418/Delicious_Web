using System;
using System.Collections.Generic;

#nullable disable

namespace prjDelicious.Models
{
    public partial class ThashtagRecord
    {
        public int HashTagFolderId { get; set; }
        public int HashTagId { get; set; }
        public int RecipeId { get; set; }

        public virtual Thashtag HashTag { get; set; }
        public virtual Trecipe Recipe { get; set; }
    }
}
