using Microsoft.AspNetCore.Mvc;
using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace prjDelicious.Controllers
{
    public class LikeRecipeController : Controller
    {
        private readonly DeliciousContext _deliciousContext;
        public LikeRecipeController(DeliciousContext deliciousContext)
        {
            _deliciousContext = deliciousContext;
        }
        
        public IActionResult Index(int id)
        {//新增檢視>View            
            return View();
        }

        //喜歡的食譜類別
        public IActionResult LikeRecipeCategory(int id)//memberid
        {
            var likerecipe_category = _deliciousContext.TlikeRecipes.Where(c => c.MemberId == id).Select(c => new
            {
                c.Recipe.RecipeCategory.RecipeCategoryId,
                c.Recipe.RecipeCategory.RecipeCategory
            }).Distinct();
            return Json(likerecipe_category);
        }

        //喜歡的食譜
        public IActionResult LikeRecipe(int id)//memberid
        {
            var likerecipe = _deliciousContext.TlikeRecipes.Where(m => m.MemberId == id).Select(m => new
            {
                m.RecipeLikedId,
                m.RecipeId,
                m.Recipe.RecipeCategory.RecipeCategory,
                m.Recipe.RecipeName,
                m.Recipe.RecipeDescription,
                m.Recipe.ForHowMany,
                m.Recipe.TimeNeed,
                m.Recipe.Picture
            });            
            return Json(likerecipe);
        }

        //喜歡的食譜類別的食譜
        public IActionResult selLikeCat(int memberid,int categoryid)
        {
            var sellikecat = _deliciousContext.TlikeRecipes.Where(m => m.MemberId == memberid && m.Recipe.RecipeCategoryId==categoryid).Select(m => new
            {
                m.RecipeLikedId,
                m.RecipeId,
                m.Recipe.RecipeCategory.RecipeCategory,
                m.Recipe.RecipeName,
                m.Recipe.RecipeDescription,
                m.Recipe.ForHowMany,
                m.Recipe.TimeNeed,
                m.Recipe.Picture
            });
            return Json(sellikecat);
        }

        //Recipe食譜頁面上不喜歡此食譜時按了按鈕可做刪除
        public bool lookforLikeRecipeid(int memberid, int recipeid)
        {
            var q = _deliciousContext.TlikeRecipes.Where(m => m.MemberId == memberid && m.RecipeId == recipeid).FirstOrDefault();
            try
            {
                _deliciousContext.TlikeRecipes.Remove(q);
                _deliciousContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            //Delete(q);
        }

        //刪除喜歡的食譜
        public bool Delete(int likerecipeid)
        {
            var recipedelete = _deliciousContext.TlikeRecipes.FirstOrDefault(r => r.RecipeLikedId == likerecipeid);
            try
            {
                _deliciousContext.TlikeRecipes.Remove(recipedelete);
                _deliciousContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //關鍵字搜尋食譜
        public IActionResult Search(int memberid, string keyword)
        {
            var search = _deliciousContext.TlikeRecipes.Where(s => s.MemberId == memberid && (s.Recipe.RecipeCategory.RecipeCategory.Contains(keyword) || s.Recipe.RecipeName.Contains(keyword))).Select(s => new
            {
                s.RecipeLikedId,
                s.RecipeId,
                s.Recipe.RecipeCategory.RecipeCategory,
                s.Recipe.RecipeName,
                s.Recipe.RecipeDescription,
                s.Recipe.ForHowMany,
                s.Recipe.TimeNeed,
                s.Recipe.Picture
            });            
            return Json(search);
        }

        //使用者將食譜加至喜歡
        public string AddIntoLike(int memberid,int recipeid)
        {//判斷使用者加入的食譜是否有重複喜歡
            int countsame = CountReapt(memberid, recipeid);

            if (countsame > 0)
            {
                return "此食譜重複新增";
            }
            else
            {
                TlikeRecipe newLike = new TlikeRecipe();
                newLike.MemberId = memberid;
                newLike.RecipeId = recipeid;
                _deliciousContext.TlikeRecipes.Add(newLike);
                try
                {
                    _deliciousContext.SaveChanges();
                    return "食譜已加至喜歡";
                }
                catch (Exception)
                {
                    return "食譜加入喜歡失敗";
                }
            }          
        }
        //判斷使用者加入的食譜是否有重複喜歡
        public int CountReapt(int memberid, int recipeid)
        {
            return _deliciousContext.TlikeRecipes.Where(m => m.MemberId == memberid && m.RecipeId == recipeid).Count();
        }
    }
}
