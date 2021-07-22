using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.Models
{
    public class TForIMG
    {

        public Tstep step { get; set; }
        public Trecipe recipe { get; set; }

        TForIMG() 
        {
            step = new Tstep();
            recipe = new Trecipe();
        }

        public string recipeIMG
        {
            get { return recipe.Picture; }
            set { recipe.Picture = value; }
        }

        public string stepIMG
        {
            get { return step.Picture; }
            set { step.Picture = value; }
        }

    }
}
