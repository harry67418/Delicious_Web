using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class CMemberRankViewModel
    {
        public List<Tmember> _members { get; set; }
        public List<CRecipeCountViewModel> _howmany { get; set; }
        public List<CLikeRecipeRankViewModel> _recipes { get; set; }
        public List<CCollectRecipeViewModel> _collections { get; set; } 

        public CMemberRankViewModel()
        {
            _members = new List<Tmember>();
            _recipes = new List<CLikeRecipeRankViewModel>();
            _collections = new List<CCollectRecipeViewModel>();
            _howmany = new List<CRecipeCountViewModel>();
        }
        public string FigureSrc { get; set; }
        public string RecipesSrc { get; set; }
    }
}

    
    

