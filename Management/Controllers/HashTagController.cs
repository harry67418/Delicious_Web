using Management.Models;
using Management.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Controllers
{
    public class HashTagController : Controller
    {
        private readonly DeliciousContext _delicious;
        private IWebHostEnvironment _host;

        public HashTagController(DeliciousContext deliciousContext, IWebHostEnvironment hostEnvironment)
        {
            _delicious = deliciousContext;
            _host = hostEnvironment;
        }
        public IActionResult Index(string txtSearch)
        {
            var hashtags = _delicious.Thashtags.Select(t => new
            {
                t.HashtagId,
                t.Hasgtag,
                TagRecipes = t.ThashtagRecords.Count
            }).AsEnumerable();
            if (txtSearch != null)
            {
                string[] Searches = txtSearch.Split();
                foreach (string s in Searches)
                {
                    hashtags = hashtags.Where(t => t.Hasgtag.Contains(s)).ToList();
                }
            }
            var hashtaglist = hashtags.Select(t => new CHashTagViewModel()
            {
                HashtagId = t.HashtagId,
                Hashtag = t.Hasgtag,
                TagRecipes = t.TagRecipes
            }).AsEnumerable();
            return View(hashtaglist);
        }
        public IActionResult List()
        {
            var hashtags = _delicious.Thashtags.Select(t => new
            {
                t.HashtagId,
                t.Hasgtag
            }).AsEnumerable();
            return View(hashtags);
        }
        public IActionResult SerachHashTag(string txtSearch)
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
        public IActionResult SerachRecipe(string txtSearch)
        {
            var Recipes = _delicious.Trecipes.Select(r => new
            {
                r.RecipeId,
                r.RecipeName,
                r.RecipeCategory.RecipeCategory,
                r.RecipeDescription,
                r.Views,
                Tags = _delicious.ThashtagRecords.Where(rc => rc.RecipeId == r.RecipeId).Select(t => t.HashTag.Hasgtag).ToList(),
                Ingds = _delicious.TingredientRecords.Where(rc => rc.RecipeId == r.RecipeId).Select(t => t.Ingredient.Ingredient).ToList(),
            }).ToList();
            if (txtSearch != null)
            {
                string[] Searches = txtSearch.Split();
                foreach (string s in Searches)
                {
                    Recipes = Recipes.Where(r => r.RecipeName.Contains(s) || r.RecipeCategory.Contains(s) || r.Tags.Any(t => t.Contains(s)) || r.Ingds.Any(i => i.Contains(s))).ToList();
                }
            }
            return Json(Recipes);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Thashtag hashtag)
        {
            _delicious.Add(hashtag);
            _delicious.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var hashtag = _delicious.Thashtags.Where(t => t.HashtagId == id).FirstOrDefault();
            return View(hashtag);
        }
        [HttpPost]
        public IActionResult Edit(Thashtag tag)
        {
            var etag = _delicious.Thashtags.Where(t => t.HashtagId == tag.HashtagId).FirstOrDefault();
            etag.HashtagId = tag.HashtagId;
            etag.Hasgtag = tag.Hasgtag;
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
        public IActionResult CreateTag(string tagName)
        {
            if(_delicious.Thashtags.Count(t => t.Hasgtag == tagName) == 0)
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
            else
            {
                string OK = "repeat";
                return Json(OK);
            }
        }
        public IActionResult DeleteTag(int tagId)
        {
            var tag = _delicious.Thashtags.Where(t => t.HashtagId == tagId).FirstOrDefault();
            var records = _delicious.ThashtagRecords.Where(t => t.HashTagId == tagId);
            _delicious.Thashtags.Remove(tag);
            _delicious.ThashtagRecords.RemoveRange(records);
            _delicious.SaveChanges();
            var hashtags = _delicious.Thashtags.Select(t => new
            {
                t.HashtagId,
                t.Hasgtag
            }).AsEnumerable();
            return Json(hashtags);
        }
        public IActionResult EditTag(int tagId, string tagName)
        {
            if (_delicious.Thashtags.Count(t => t.Hasgtag == tagName) == 0)
            {
                var tag = _delicious.Thashtags.Where(t => t.HashtagId == tagId).FirstOrDefault();
                tag.Hasgtag = tagName;
                _delicious.SaveChanges();
                var hashtags = _delicious.Thashtags.Select(t => new
                {
                    t.HashtagId,
                    t.Hasgtag
                }).AsEnumerable();
                return Json(hashtags);
            }
            else
            {
                string OK = "repeat";
                return Json(OK);
            }
        }
        public IActionResult schAT(string txt)
        {
            var hashtags = _delicious.Thashtags.Select(t => new
            {
                t.HashtagId,
                t.Hasgtag,
                RecipeCount = t.ThashtagRecords.Count
            }).AsEnumerable();
            if (txt != null)
            {
                string[] searches = txt.Split();
                foreach (string s in searches)
                {
                    hashtags = hashtags.Where(t => t.Hasgtag.Contains(s)).ToList();
                }
            }
            return Json(hashtags);
        }
        public IActionResult schAR(int id, string txt)
        {
            var recipes = _delicious.Trecipes.Select(r => new
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
                Ingds = _delicious.TingredientRecords.Where(rc => rc.RecipeId == r.RecipeId).Select(t => t.Ingredient.Ingredient).ToList(),
                TagFlag = _delicious.ThashtagRecords.Where(rc => (rc.RecipeId == r.RecipeId) && (rc.HashTagId == id)).Count()
            }).ToList();
            if (txt != null)
            {
                string[] searches = txt.Split();
                foreach (string s in searches)
                {
                    recipes = recipes.Where(r => r.RecipeName.Contains(s) || r.RecipeCategory.Contains(s) || r.Member.Contains(s) || r.Tags.Any(t => t.Contains(s)) || r.Ingds.Any(i => i.Contains(s))).ToList();
                }
            }
            return Json(recipes);
        }
        public IActionResult schTR(int id, string txt)
        {
            var TagRecipeIds = _delicious.ThashtagRecords.Where(r => r.HashTagId == id).Select(t => t.RecipeId).ToArray<int>();
            var recipes = _delicious.Trecipes.Where(r => TagRecipeIds.Contains(r.RecipeId)).Select(r => new
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
                Ingds = _delicious.TingredientRecords.Where(rc => rc.RecipeId == r.RecipeId).Select(t => t.Ingredient.Ingredient).ToList(),
                TagFlag = _delicious.ThashtagRecords.Where(rc => (rc.RecipeId == r.RecipeId) && (rc.HashTagId == id)).Count()
            }).ToList();
            if (txt != null)
            {
                string[] searches = txt.Split();
                foreach (string s in searches)
                {
                    recipes = recipes.Where(r => r.RecipeName.Contains(s) || r.RecipeCategory.Contains(s) || r.Member.Contains(s) || r.Tags.Any(t => t.Contains(s)) || r.Ingds.Any(i => i.Contains(s))).ToList();
                }
            }
            return Json(recipes);
        }
        public IActionResult schSR(int id, string txt, string rids)
        {

            string[] jids = rids.Split(',');
            int[] ids = jids.Select(r => Int32.Parse(r)).ToArray<int>();
            if (id != 0)
            {
                var TagRecipeIds = _delicious.ThashtagRecords.Where(r => r.HashTagId == id).Select(t => t.RecipeId).ToArray<int>();
            }
            var recipes = _delicious.Trecipes.Where(r => ids.Contains(r.RecipeId)).Select(r => new
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
            if (txt != null)
            {
                string[] searches = txt.Split();
                foreach (string s in searches)
                {
                    recipes = recipes.Where(r => r.RecipeName.Contains(s) || r.RecipeCategory.Contains(s) || r.Member.Contains(s) || r.Tags.Any(t => t.Contains(s)) || r.Ingds.Any(i => i.Contains(s))).ToList();
                }
            }
            return Json(recipes);
        }
        public IActionResult DeleteRecipeTag(int rid, int tid)
        {
            var record = _delicious.ThashtagRecords.Where(r => (r.RecipeId == rid) && (r.HashTagId == tid)).FirstOrDefault();
            _delicious.ThashtagRecords.Remove(record);
            _delicious.SaveChanges();
            var RecipeTagIds = _delicious.ThashtagRecords.Where(r => r.RecipeId == rid).Select(t => t.HashTagId).ToArray<int>();
            bool OK = true;
            return Json(OK);
        }
        public IActionResult AddRecipeTag(int rid, int tid)
        {
            if(_delicious.ThashtagRecords.Count(r => (r.RecipeId == rid) && (r.HashTagId == tid)) == 0)
            {
                ThashtagRecord record = new ThashtagRecord();
                record.RecipeId = rid;
                record.HashTagId = tid;
                _delicious.Add(record);
                _delicious.SaveChanges();
            }
            bool OK = true;
            return Json(OK);
        }
    }
}
