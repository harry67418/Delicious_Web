using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class CCollectRecipeViewModel
    {
        public int RecipeId { get; set; }
        public string recipePicture { get; set; }
        public string RecipeName { get; set; }
        public string Nickname { get; set; }
        public int COUNT { get; set; }
    }
}
