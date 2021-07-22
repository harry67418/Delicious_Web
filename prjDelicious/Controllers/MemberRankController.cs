using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using prjDelicious.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.Models
{
    public class MemberRankController : Controller
    {
        private readonly DeliciousContext _deliciousContext;
        private readonly IOptions<CSrcSetting> _CSrcSetting;

        public MemberRankController(DeliciousContext deliciousContext,IOptions<CSrcSetting> CSrcSetting)
        {
            _deliciousContext = deliciousContext;
            _CSrcSetting = CSrcSetting;
        }
        public IActionResult List()
        {
            CMemberRankViewModel model = new CMemberRankViewModel();

            //會員積分排行榜
            var members = _deliciousContext.Tmembers.Select(m => m).OrderByDescending(m => m.PersonalRankScore).Take(5).ToList();
            model._members = members;            
            
            //會員發表食譜累積榜
            var totalrecipe = _deliciousContext.Trecipes.GroupBy(t => t.MemberId).OrderByDescending(t => t.Count()).Select(t => new
            {
                MemberID = t.Key,
                RecipeCount = t.Count()
            }).Take(5);            
            foreach (var item in totalrecipe)
            {
                CRecipeCountViewModel modelcount = new CRecipeCountViewModel();
                modelcount.MemberId = item.MemberID;
                modelcount.COUNT = item.RecipeCount;
                model._howmany.Add(modelcount);
            }
            foreach (var item in model._howmany)
            {
                var q = _deliciousContext.Tmembers.Single(n => n.MemberId == item.MemberId);
                item.Nickname = q.Nickname;
                item.Figure = q.Figure;                 
            }

            //喜歡的食譜排行榜
            var recipes = _deliciousContext.TlikeRecipes.GroupBy(r => r.RecipeId).OrderByDescending(r => r.Count()).Select(r => new
            {
                LikeRecipeId = r.Key,
                LikeRecipeCount = r.Count()
            }).Take(5);            
            foreach (var item in recipes)
            {
                CLikeRecipeRankViewModel x = new CLikeRecipeRankViewModel();
                x.RecipeId = item.LikeRecipeId;
                x.COUNT = item.LikeRecipeCount;
                model._recipes.Add(x);
            }
            foreach (var item in model._recipes)
            {
                var q = _deliciousContext.Trecipes.FirstOrDefault(n => n.RecipeId == item.RecipeId);
                item.recipePicture = q.Picture;
                item.Nickname = q.Member.Nickname;
                item.RecipeName = q.RecipeName;
            }

            //收藏的食譜排行榜
            var collections = _deliciousContext.Tcollections.GroupBy(c => c.ReicipeId).OrderByDescending(c => c.Count()).Select(c => new
            {
                collRecipeId = c.Key,
                collRecipeCOUNT = c.Count()
            }).Take(5);            
            foreach(var item in collections)
            {
                CCollectRecipeViewModel x = new CCollectRecipeViewModel();
                x.RecipeId = item.collRecipeId;
                x.COUNT = item.collRecipeCOUNT;
                model._collections.Add(x);
            }
            foreach(var item in model._collections)
            {
                var q = _deliciousContext.Tcollections.FirstOrDefault(n => n.ReicipeId == item.RecipeId);
                item.recipePicture = q.Reicipe.Picture;
                item.Nickname = q.Reicipe.Member.Nickname;
                item.RecipeName = q.Reicipe.RecipeName;
            }

            model.FigureSrc = _CSrcSetting.Value.FigureSrc;
            model.RecipesSrc = _CSrcSetting.Value.RecipesSrc;
            return View(model);            
        }
    }
}
