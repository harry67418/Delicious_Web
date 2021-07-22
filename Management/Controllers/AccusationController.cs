using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Management.Models;
using Microsoft.Extensions.Logging;
using Management.ViewModel;

namespace Management.Controllers
{
    public class AccusationController : Controller
    {

        /// <summary>
        /// 舉報介面
        /// </summary>
        private readonly DeliciousContext _context;
        //private readonly IWebHostEnvironment _hostingEnvironment;

        private readonly ILogger<HomeController> _logger;

        public AccusationController(ILogger<HomeController> logger, DeliciousContext deliciousContext)
        {
            _logger = logger;
            _context = deliciousContext;
            // _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            List<CVAccusationcs> ListcCAccusationcs = new List<CVAccusationcs>();
            var n = from data in _context.Set<Taccusation>()
                    join data1 in _context.Set<Trecipe>()
                        on data.AccusedId equals data1.RecipeId.ToString()
                    join data2 in _context.Set<TfeedbackProgress>()
                    on data.ProgressId equals data2.ProgressId
                    where data.AccusedAvatar == "1"
                    select new { data.AccusationRightId, data.MemberId, data.Accuse.Accusation, data.AccusedAvatar, data1.RecipeName, data2.ProgressContent, data1.DisVisible };
            var n2 = from data in _context.Set<Taccusation>()
                    join data1 in _context.Set<TcommentSection>()
                        on data.AccusedId equals data1.CommentId.ToString()
                    join data2 in _context.Set<TfeedbackProgress>()
                    on data.ProgressId equals data2.ProgressId
                    where data.AccusedAvatar == "2"
                    select new { data.AccusationRightId, data.MemberId, data.Accuse.Accusation, data.AccusedAvatar, data1.Comment, data2.ProgressContent, data1.DisVisible };
            foreach (var item in n)
            {
                CVAccusationcs _cAccusationcs = new CVAccusationcs();
                _cAccusationcs.AccusationRightId = item.AccusationRightId;
                _cAccusationcs.MemberId = item.MemberId;
                _cAccusationcs.Accusation = item.Accusation;
                _cAccusationcs.AccusedAvatar = "食譜"; 
                _cAccusationcs.AccusedName = item.RecipeName;
                _cAccusationcs.ProgressStatu = item.ProgressContent;
                if (item.DisVisible)
                {
                    _cAccusationcs.DisVisible = "凍結";
                }
                else
                {
                    _cAccusationcs.DisVisible = "正常";
                }
                ListcCAccusationcs.Add(_cAccusationcs);
            }
            foreach (var item in n2)
            {
                CVAccusationcs _cAccusationcs = new CVAccusationcs();
                _cAccusationcs.AccusationRightId = item.AccusationRightId;
                _cAccusationcs.MemberId = item.MemberId;
                _cAccusationcs.Accusation = item.Accusation;
                _cAccusationcs.AccusedAvatar = "討論區";   
                _cAccusationcs.AccusedName = item.Comment;
                _cAccusationcs.ProgressStatu = item.ProgressContent;
                if (item.DisVisible)
                {
                    _cAccusationcs.DisVisible = "凍結";
                }
                else
                {
                    _cAccusationcs.DisVisible = "正常";
                }
                ListcCAccusationcs.Add(_cAccusationcs);
            }
            return View(ListcCAccusationcs);
        }
        public IActionResult member_accusation_create(int id)
        {

            Taccusation taccusation = new Taccusation();
            taccusation.MemberId = id;
           
            return View(taccusation);
        }
        [HttpPost]
        public IActionResult member_accusation_create(int MemberId, string AccusedId, string AccuseId, string AccusedAvatar)
        {
              
            CAccusation cAccusation = new CAccusation(_context);
            if (AccusedAvatar == "討論區")
            {
                cAccusation.insertCommentAccusation(MemberId, AccusedId, AccuseId);
            }
            else
            {
                cAccusation.insertRecipeAccusation(MemberId, AccusedId, AccuseId);
            }

            

            return RedirectToAction("Index");
        }
        public IActionResult member_deForze(int id, string AccusedAvatar)
        {
            CAccusation cAccusation = new CAccusation(_context);

            if (AccusedAvatar == "討論區")
            {
                cAccusation.DisVisible_Comment(id, false);
            }
            else
            {
                cAccusation.DisVisible_Recipe(id, false);
            }

            return RedirectToAction("index");
        }
        public IActionResult member_Forze(int id, string AccusedAvatar)
        {
            CAccusation cAccusation = new CAccusation(_context);

            if (AccusedAvatar == "討論區")
            {
                cAccusation.DisVisible_Comment(id, true);
            }
            else
            {
                cAccusation.DisVisible_Recipe(id, true);
            }

            return RedirectToAction("index");
        }
        public IActionResult search_recipe()
        {
            var RecipeCategory = _context.Trecipes;
            return View(RecipeCategory);
        }
        public IActionResult search_Comment()
        {
            var Comment_list = _context.TcommentSections;
            return View(Comment_list);
        }
    }
}
