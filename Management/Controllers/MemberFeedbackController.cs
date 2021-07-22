using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Management.Models;
using Management.ViewModel;

namespace Management.Controllers
{
    public class MemberFeedbackController : Controller
    {
        /// <summary>
        /// 客訴介面
        /// </summary>
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly DeliciousContext _context;

        public MemberFeedbackController(DeliciousContext context, IWebHostEnvironment webHostEnvironment)
        {
            this._webHostEnvironment = webHostEnvironment;
            this._context = context;
        }
        public IActionResult Index()
        {
            List<CMemberFeedbackcs> ListcMemberFeedbackcs = new List<CMemberFeedbackcs>();

            var m_view = _context.Tfeedbacks.Select(n => new
            {
                n.Comment,
                n.Contents,
                n.FeedbackCategory.FeedbackCategory,
                n.FeedbackContent,
                n.FeedbackDate,
                n.MemberId,
                n.FeedbackId,
                n.Progress.ProgressContent,
                n.Picture
            });
            foreach (var item in m_view)
            {
                CMemberFeedbackcs cMemberFeedbackcs = new CMemberFeedbackcs();
                cMemberFeedbackcs.FeedbackId = item.FeedbackId;
                cMemberFeedbackcs.FeedbackCategory = item.FeedbackCategory;
                cMemberFeedbackcs.MemberId = item.MemberId;
                cMemberFeedbackcs.Contents = item.Contents;
                cMemberFeedbackcs.ProgressContent = item.ProgressContent;
                cMemberFeedbackcs.Comment = item.Comment;
                cMemberFeedbackcs.FeedbackDate = item.FeedbackDate;
                cMemberFeedbackcs.FeedbackContent = item.FeedbackContent;
                cMemberFeedbackcs.Picture = item.Picture;
                ListcMemberFeedbackcs.Add(cMemberFeedbackcs);
            }

            return View(ListcMemberFeedbackcs);
        }
        public IActionResult ReplyFeedBack(int id)
        {
            Tfeedback tfeedback = new Tfeedback() { FeedbackId = id };
            return View(tfeedback);
        }
        [HttpPost]
        public IActionResult ReplyFeedBack(Tfeedback _tfeedback)
        {
            CMemberFeedback cMemberFeedback = new CMemberFeedback(_context);
            cMemberFeedback.insertFeedbackComment(_tfeedback.FeedbackId, _tfeedback.FeedbackContent);
            return RedirectToAction("index");
        }
    }
}
