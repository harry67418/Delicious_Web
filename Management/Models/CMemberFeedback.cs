using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Models
{
    public class CMemberFeedback
    {
        private readonly DeliciousContext _context;
        public CMemberFeedback(DeliciousContext context)
        {

            this._context = context;
        }
        public void insertFeedbackComment ( int FeedbackId, string FeedbackContent)
        {
           var reslult = (from n in _context.Tfeedbacks
                       where n.FeedbackId == FeedbackId
                       select n ).FirstOrDefault();
            reslult.FeedbackContent = FeedbackContent;
            reslult.ProgressId = 3;
            _context.SaveChanges();

         
        }
    }
}
