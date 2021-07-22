using System;
using System.Collections.Generic;

#nullable disable

namespace WebChart.Models
{
    public partial class Trecipe
    {
        public Trecipe()
        {
            Tcollections = new HashSet<Tcollection>();
            TcommentSections = new HashSet<TcommentSection>();
            ThashtagRecords = new HashSet<ThashtagRecord>();
            TingredientRecords = new HashSet<TingredientRecord>();
            TlikeRecipes = new HashSet<TlikeRecipe>();
            Tsteps = new HashSet<Tstep>();
            TwishLists = new HashSet<TwishList>();
        }

        public int RecipeId { get; set; }
        public DateTime PostTime { get; set; }
        public int RecipeCategoryId { get; set; }
        public int MemberId { get; set; }
        public string RecipeName { get; set; }
        public string RecipeDescription { get; set; }
        public int ForHowMany { get; set; }
        public int TimeNeed { get; set; }
        public int Views { get; set; }
        public string Picture { get; set; }
        public string Video { get; set; }
        public bool DisVisible { get; set; }
        public bool DeleteOrNot { get; set; }

        public virtual Tmember Member { get; set; }
        public virtual TrecipeCategory RecipeCategory { get; set; }
        public virtual ICollection<Tcollection> Tcollections { get; set; }
        public virtual ICollection<TcommentSection> TcommentSections { get; set; }
        public virtual ICollection<ThashtagRecord> ThashtagRecords { get; set; }
        public virtual ICollection<TingredientRecord> TingredientRecords { get; set; }
        public virtual ICollection<TlikeRecipe> TlikeRecipes { get; set; }
        public virtual ICollection<Tstep> Tsteps { get; set; }
        public virtual ICollection<TwishList> TwishLists { get; set; }
    }
}
