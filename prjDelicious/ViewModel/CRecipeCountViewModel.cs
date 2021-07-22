using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class CRecipeCountViewModel
    {
        public int MemberId { get; set; }
        public string Nickname { get; set; }
        public string Figure { get; set; }
        public int COUNT { get; set; } //會員排行榜用---算某會員累積發表過多少食譜
        public int RecipeId { get; set; }
    }
}
