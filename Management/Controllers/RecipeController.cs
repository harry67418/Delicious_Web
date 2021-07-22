using Management.Models;
using Management.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Controllers
{
    [Authorize]
    public class RecipeController : Controller
    {
      
        private readonly DeliciousContext _deliciousContext;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public RecipeController(IWebHostEnvironment hostingEnvironment, DeliciousContext deliciousContext)
        {
            _deliciousContext = deliciousContext;
            _hostingEnvironment = hostingEnvironment;
          
        }
        public IActionResult Index(int pg,int category,string keywords,string recipeid)
        {
            var q = _deliciousContext.Trecipes.OrderByDescending(n=>n.PostTime).Select(n => new {
            n,
            n.RecipeCategory,
            n.Member,
            n.RecipeId
            });
            if (category != 0)
            {
                q = q.Where(n => n.n.RecipeCategoryId == category);
            }
            if (keywords != null)
            {
                q = q.Where(n => n.Member.MemberId.ToString().Contains(keywords) || n.Member.Email.Contains(keywords) || n.Member.CellNumber.Contains(keywords));
                ViewBag.keywords = keywords;
            }
            if (recipeid != null)
            {
                q = q.Where(n => n.RecipeId==Convert.ToInt32(recipeid));
                ViewBag.keywords = recipeid;

            }
            ViewBag.cate = category;

            CRecipeViewModel crecipelist = new CRecipeViewModel();
            crecipelist.total = q.Count();
            foreach(var item in q)
            {
                CRecipeItemViewModel trecipeitem = new CRecipeItemViewModel();
                trecipeitem.trecipe = item.n;
                trecipeitem.trecipeCategory = item.RecipeCategory;
                crecipelist.list.Add(trecipeitem);

            }
            foreach (var item in crecipelist.list)
            {
                var like = _deliciousContext.TlikeRecipes.Where(n => n.RecipeId == item.trecipe.RecipeId).Count();
                item.countlike = like;

            }
            foreach (var item in crecipelist.list)
            {
                var comment = _deliciousContext.TcommentSections.Where(n => n.RecipeId == item.trecipe.RecipeId).Count();
                item.countcomment = comment;

            }

            const int pagesize= 10;
            if (pg < 1)
            { pg = 1; }
            int resCount = crecipelist.list.Count();

            CPager pager = new CPager(resCount,pg,pagesize);
            int recSkip = (pg - 1) * pagesize;
            crecipelist.list = crecipelist.list.Skip(recSkip).Take(pager.Pagesize).ToList();
            this.ViewBag.pager = pager;

            var cates = _deliciousContext.TrecipeCategories.Select(n => n);
            foreach (var item in cates)
            {
                CRecipeCategoryViewModel onecate = new CRecipeCategoryViewModel();
                onecate.category = item;
                crecipelist.caterecipelist.Add(onecate);
            }
            
            return View(crecipelist);
        }
        public IActionResult Savedeletestatus(int recipeid) 
        {
            var q = _deliciousContext.Trecipes.Where(n => n.RecipeId == recipeid).FirstOrDefault();
            if (q.DeleteOrNot) 
            { q.DeleteOrNot = false; }
           else
            { q.DeleteOrNot = true; }
            _deliciousContext.SaveChanges();
            return NoContent();
        }
    }
}
