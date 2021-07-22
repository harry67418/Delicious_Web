using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjDelicious.Models;
using prjDelicious.ViewModel;

namespace prjDelicious.Controllers
{
    public class TfeedbacksController : Controller
    {
        /// <summary>
        /// 客訴controller
        /// </summary>
        private readonly DeliciousContext _context;

        public TfeedbacksController(DeliciousContext context)
        {
            _context = context;
        }

        // GET: Tfeedbacks
        //public async Task<IActionResult> Index()
        //{
        //    var deliciousContext = _context.Tfeedbacks.Include(t => t.FeedbackCategory).Include(t => t.Member).Include(t => t.Progress);
        //    return View(await deliciousContext.ToListAsync());
        //}

        // GET: Tfeedbacks/5
        public async Task<IActionResult> Index()
        {
            string loginid = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
            if (loginid == null || loginid == "")
            {
                return RedirectToAction("Login", "homepage");
            }
            else
            {
                int memberID = 0;
                memberID = Convert.ToInt32(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
                var ing = _context.Tmembers.Where(n => n.MemberId == memberID).Select(n => new { n.MemberName }).First();
                if (ing == null) { return RedirectToAction("Login", "homepage"); }

                var deliciousContext = _context.Tfeedbacks.Include(t => t.FeedbackCategory).Include(t => t.Member).Include(t => t.Progress).Where(t => t.MemberId == memberID);
                return View(await deliciousContext.ToListAsync());
            }
        }

        // GET: Tfeedbacks/Create
        public IActionResult Create()
        {
            string loginid = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
            if (loginid == null || loginid == "")
            {
                return RedirectToAction("Login", "homepage");
            }
            else
            {
                
                ViewData["FeedbackCategoryId"] = new SelectList(_context.TfeedbackCategories, "FeedbackCategoryId", "FeedbackCategory");
                ViewData["MemberId"] = loginid;
                ViewData["ProgressId"] = new SelectList(_context.TfeedbackProgresses, "ProgressId", "ProgressContent");
            }
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Tfeedback tfeedback)
        {
            if(string.IsNullOrEmpty( tfeedback.Contents ))
            {
                string loginid = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
                ViewData["FeedbackCategoryId"] = new SelectList(_context.TfeedbackCategories, "FeedbackCategoryId", "FeedbackCategory");
                ViewData["MemberId"] = loginid;
               
                return View(tfeedback);
            }
            if (tfeedback.MemberId == 0) { return RedirectToAction("index"); }
            tfeedback.FeedbackDate = DateTime.Now;
            tfeedback.ProgressId = 1;
            _context.Tfeedbacks.Add(tfeedback);
            _context.SaveChanges();
            return RedirectToAction("index");
        }

        // GET: Tfeedbacks/Edit/5
       

        private bool TfeedbackExists(int id)
        {
            return _context.Tfeedbacks.Any(e => e.FeedbackId == id);
        }
    }
}
