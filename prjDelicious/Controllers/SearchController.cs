using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using prjDelicious.Models;
using prjDelicious.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.Controllers
{
    public class SearchController : Controller
    {
        private readonly DeliciousContext _delicious;
        private IWebHostEnvironment _host;

        public SearchController(DeliciousContext deliciousContext, IWebHostEnvironment hostEnvironment)
        {
            _delicious = deliciousContext;
            _host = hostEnvironment;
        }
        public IActionResult RecipeList(string txtSearch)
        {
            ViewBag.txt = txtSearch;
            return View();
        }
        public IActionResult TagCloud()
        {
            return View();
        }
        public IActionResult TagList(int id)
        {
            ViewBag.tagid = id;
            ViewBag.tag = _delicious.Thashtags.FirstOrDefault(t => t.HashtagId == id).Hasgtag;
            ViewBag.views = _delicious.Thashtags.FirstOrDefault(t => t.HashtagId == id).ThashtagRecords.Sum(r => r.Recipe.Views);
            return View();
        }
        public IActionResult TagRecipe(int id)
        {
            var Recipes = _delicious.Trecipes.Where(r => r.ThashtagRecords.Any(t => t.HashTagId == id)).Select(r => new
            {
                r.RecipeId,
                r.RecipeName,
                r.RecipeCategory.RecipeCategory,
                r.RecipeDescription,
                r.Picture,
                r.Views,
                r.DeleteOrNot,
                r.DisVisible,
                Time = r.PostTime.ToShortDateString(),
                Member = _delicious.Tmembers.FirstOrDefault(m => m.MemberId == r.MemberId).Nickname,
                Likes = r.TlikeRecipes.Count,
                Tags = _delicious.ThashtagRecords.Where(rc => rc.RecipeId == r.RecipeId).Select(t => t.HashTag.Hasgtag).ToList(),
                Ingds = _delicious.TingredientRecords.Where(rc => rc.RecipeId == r.RecipeId).Select(t => t.Ingredient.Ingredient).ToList()
            }).ToList();
            return Json(Recipes);
        }
        public IActionResult SearchRecipe(string txtSearch)
        {
            var Recipes = _delicious.Trecipes.Select(r => new
            {
                r.RecipeId,
                r.RecipeName,
                r.RecipeCategory.RecipeCategory,
                r.RecipeDescription,
                r.Picture,
                r.Views,
                r.DeleteOrNot,
                r.DisVisible,
                Time = r.PostTime.ToShortDateString(),
                Member = _delicious.Tmembers.FirstOrDefault(m => m.MemberId == r.MemberId).Nickname,
                Likes = r.TlikeRecipes.Count,
                Tags = _delicious.ThashtagRecords.Where(rc => rc.RecipeId == r.RecipeId).Select(t => t.HashTag.Hasgtag).ToList(),
                Ingds = _delicious.TingredientRecords.Where(rc => rc.RecipeId == r.RecipeId).Select(t => t.Ingredient.Ingredient).ToList()
            }).ToList();
            if (txtSearch != null)
            {
                string[] Searches = txtSearch.Split();
                foreach (string s in Searches)
                {
                    Recipes = Recipes.Where(r => r.RecipeName.Contains(s) || r.RecipeCategory.Contains(s) || r.Member.Contains(s) || r.Tags.Any(t => t.Contains(s)) || r.Ingds.Any(i => i.Contains(s))).ToList();
                }
            }
            return Json(Recipes);
        }
        public IActionResult GetCategories()
        {
            var categories = _delicious.TrecipeCategories.Select(c => new
            {
                c.RecipeCategoryId,
                c.RecipeCategory
            }).ToList();
            return Json(categories);
        }
        public IActionResult GetHashTags()
        {
            var hashtags = _delicious.Thashtags.Select(t => new
            {
                t.HashtagId,
                t.Hasgtag,
                Views = t.ThashtagRecords.Sum(r => r.Recipe.Views)
            }).ToList();
            return Json(hashtags);
        }

        public IActionResult MemberList(string txtSearch)
        {
            ViewBag.txt = txtSearch;
            return View();
        }
        public IActionResult SearchMember(string txtSearch)
        {
            var Members = _delicious.Tmembers.Select(m => new
            {
                m.MemberId,
                m.MemberName,
                m.Nickname,
                m.Info,
                m.Figure,
                Time = m.RegisterTime.ToShortDateString(),
                Views = m.Trecipes.Sum(r => r.Views),
                Score = m.PersonalRankScore,
                Recipes = _delicious.Trecipes.Where(r => r.MemberId == m.MemberId).OrderByDescending(r => r.Views).Select(r => r.RecipeName).ToList()
            }).ToList();
            if (txtSearch != null)
            {
                string[] Searches = txtSearch.Split();
                foreach (string s in Searches)
                {
                    Members = Members.Where(m => m.MemberName.Contains(s) || m.Nickname.Contains(s) || m.Info.Contains(s) || m.Recipes.Any(r => r.Contains(s))).ToList();
                }
            }
            return Json(Members);
        }
        public IActionResult TagEdit(int id)
        {
            ViewBag.id = id;
            ViewBag.txt = _delicious.Trecipes.FirstOrDefault(r => r.RecipeId == id).RecipeName;
            return View();
        }
        public IActionResult SearchHashTag(string txtSearch)
        {
            var hashtags = _delicious.Thashtags.Select(t => new
            {
                t.HashtagId,
                t.Hasgtag
            }).AsEnumerable();
            if (txtSearch != null)
            {
                string[] Searches = txtSearch.Split();
                foreach (string s in Searches)
                {
                    hashtags = hashtags.Where(t => t.Hasgtag.Contains(s)).ToList();
                }
            }
            return Json(hashtags);
        }
        public IActionResult GetRecipeTag(int id)
        {
            var RecipeTagIds = _delicious.ThashtagRecords.Where(r => r.RecipeId == id).Select(t => t.HashTagId).ToArray<int>();
            return Json(RecipeTagIds);
        }
        public IActionResult DeleteRecipeTag(int rid, int tid)
        {
            var record = _delicious.ThashtagRecords.Where(r => (r.RecipeId == rid) && (r.HashTagId == tid)).FirstOrDefault();
            _delicious.ThashtagRecords.Remove(record);
            _delicious.SaveChanges();
            var RecipeTagIds = _delicious.ThashtagRecords.Where(r => r.RecipeId == rid).Select(t => t.HashTagId).ToArray<int>();
            return Json(RecipeTagIds);
        }
        public IActionResult AddRecipeTag(int rid, int tid)
        {
            ThashtagRecord record = new ThashtagRecord();
            record.RecipeId = rid;
            record.HashTagId = tid;
            _delicious.Add(record);
            _delicious.SaveChanges();
            var RecipeTagIds = _delicious.ThashtagRecords.Where(r => r.RecipeId == rid).Select(t => t.HashTagId).ToArray<int>();
            return Json(RecipeTagIds);
        }
        public IActionResult CreateRecipeTag(string tagName)
        {
            Thashtag tag = new Thashtag();
            tag.Hasgtag = tagName;
            _delicious.Add(tag);
            _delicious.SaveChanges();
            var hashtags = _delicious.Thashtags.Select(t => new
            {
                t.HashtagId,
                t.Hasgtag
            }).AsEnumerable();
            return Json(hashtags);
        }
        public IActionResult Create(Thashtag hashtag)
        {
            _delicious.Add(hashtag);
            _delicious.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var tag = _delicious.Thashtags.Where(t => t.HashtagId == id).FirstOrDefault();
            var records = _delicious.ThashtagRecords.Where(t => t.HashTagId == id);
            _delicious.Thashtags.Remove(tag);
            _delicious.ThashtagRecords.RemoveRange(records);
            _delicious.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
