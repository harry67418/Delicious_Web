using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class CUserPageViewModel
    {
        private readonly DeliciousContext _db;

        public Tmember _member { get; set; }
        public List<TmemberFollow> follows { get; set; }
        public List<TmemberFollow> followeds { get; set; }

        public CUserPageViewModel(int id, DeliciousContext db)
        {
            _db = db;

            _member = _db.Tmembers.Where(m => m.MemberId == id).FirstOrDefault();

            like_recipe_count = _db.TlikeRecipes.Where(c => c.MemberId == _member.MemberId).Select(c => c).Count().ToString();

            my_recipe_count = _db.Trecipes.Where(r => r.MemberId == id).Select(r => r).Count().ToString();

            rankname = _member.Rank.RankName;

            follows = new List<TmemberFollow>();
            followeds = new List<TmemberFollow>();
        }

        public string rankname { get; set; }
        public string like_recipe_count { get; set; }
        public string my_recipe_count { get; set; }
        public string figureSrc { get; set; }
        public int followCount 
        { 
            get
            {
                if (follows != null) return follows.Count();
                else return 0;
            } 
        }
        public int followedCount
        {
            get
            {
                if (followeds != null) return followeds.Count();
                else return 0;
            }
        }
    }
}
