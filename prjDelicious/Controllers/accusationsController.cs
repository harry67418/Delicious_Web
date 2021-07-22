using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjDelicious.Models;

namespace prjDelicious.Controllers
{
    public class accusationsController : Controller
    {
        private readonly DeliciousContext _context;
        /// <summary>
        /// 檢舉用的
        /// RecipeAccusationCreate=>食譜檢舉
        /// CommentAccusationCreate=>評論區檢舉
        /// </summary>
        /// <param name="context"></param>
        public accusationsController(DeliciousContext context)
        {
            _context = context;
        }
        public IActionResult RecipeAccusationCreate(int id, int _AccusedID)
        {
            if (id == 0) { RedirectToAction("login", "homepage"); }
            Taccusation taccusation = new Taccusation();
            taccusation.MemberId = id;
            taccusation.AccusedId = _AccusedID.ToString();
            return View(taccusation);
        }  
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RecipeAccusationCreate(int MemberId, string AccusedId, string AccuseId)//AccusedId檢舉編號, AccuseId被檢舉項目流水號
        {
            if(AccuseId=="請選擇")
            {
                Taccusation taccusation = new Taccusation();
                taccusation.MemberId = MemberId;
                taccusation.AccusedId = AccusedId;
                return View(taccusation);
            }
            CAccusation cAccusation = new CAccusation(_context);
            if (AccusedId == "" || AccuseId == "")
            {
                return NotFound();
            }
            else
            {
                cAccusation.insertRecipeAccusation(MemberId, AccusedId, AccuseId);
            }
            return Content("食譜檢舉完成");
        }
        public IActionResult CommentAccusationCreate(int id, int _AccusedID)
        {
            if (id == 0) { RedirectToAction("login", "homepage"); }
            Taccusation taccusation = new Taccusation();
            taccusation.MemberId = id;
            taccusation.AccusedId = _AccusedID.ToString();
            return View(taccusation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CommentAccusationCreate(int MemberId, string AccusedId, string AccuseId)//AccusedId檢舉編號, AccuseId被檢舉項目流水號
        {
            if (AccuseId == "請選擇")
            {
                Taccusation taccusation = new Taccusation();
                taccusation.MemberId = MemberId;
                taccusation.AccusedId = AccusedId;
                return View(taccusation);
            }
            CAccusation cAccusation = new CAccusation(_context);
            if (AccusedId == "" || AccuseId == "")
            {
                return NotFound();
            }
            else
            {
                cAccusation.insertCommentAccusation(MemberId, AccusedId, AccuseId);
            }
            return Content("留言檢舉完成");
        }
        public IActionResult AccusationList()
        {
            var cities = _context.TaccuseContents.Select(a => new { a.Accusation });

            return Json(cities);
        }
        // GET: accusations
        public async Task<IActionResult> Index(int id)
        {
            var deliciousContext = _context.Taccusations.Include(t => t.Accuse).Include(t => t.Member).Where(t=> t.MemberId == id);
            return View(await deliciousContext.ToListAsync());
        }

       
    }
}
