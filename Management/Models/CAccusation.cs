using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Models
{
    public class CAccusation
    {
        private readonly DeliciousContext _context;
        public CAccusation(DeliciousContext context)
        {

            this._context = context;
        }
        public void insertRecipeAccusation(int MemberId, string AccusedId, string AccuseId)
        {
            int num = (from n in _context.TaccuseContents
                       where n.Accusation == AccuseId
                       select n.AccuseId).FirstOrDefault();

            Taccusation taccusation = new Taccusation();
            taccusation.MemberId = MemberId;
            taccusation.AccuseId = num;
            taccusation.AccusedAvatar = "1";
            taccusation.AccusedId = AccusedId;
            _context.Taccusations.Add(taccusation);
            _context.SaveChanges();

            //Trecipe q = (from n in _context.Trecipes
            //             where n.RecipeId == Convert.ToInt32(AccusedId)
            //             select n).FirstOrDefault();
            //q.DisVisible = true;
            //_context.SaveChanges();
        }
        public void DisVisible_Recipe(int AccusationRightId, bool DisVisible)
        {
            var result = (from data in _context.Taccusations
                          where data.AccusationRightId == AccusationRightId
                          select data.AccusedId).FirstOrDefault();
            int AccusedId = Convert.ToInt32(result);
            Trecipe q = (from n in _context.Trecipes
                         where n.RecipeId == AccusedId
                         select n).FirstOrDefault();
            q.DisVisible = DisVisible;
            _context.SaveChanges();
            Accusation_progress(AccusationRightId);
        }
        public void insertCommentAccusation(int MemberId, string AccusedId, string AccuseId)
        {
            int num = (from n in _context.TaccuseContents
                       where n.Accusation == AccuseId
                       select n.AccuseId).FirstOrDefault();

            Taccusation taccusation = new Taccusation();
            taccusation.MemberId = MemberId;
            taccusation.AccuseId = num;
            taccusation.AccusedAvatar = "2";
            taccusation.AccusedId = AccusedId;
            _context.Taccusations.Add(taccusation);
            _context.SaveChanges();

            //TcommentSection q = (from n in _context.TcommentSections
            //                     where n.CommentId == Convert.ToInt32(AccusedId)
            //                     select n).FirstOrDefault();
            //q.DisVisible = true;
            //_context.SaveChanges();
        }
        public void DisVisible_Comment(int AccusationRightId, bool DisVisible)
        {
            var result = (from data in _context.Taccusations
                          where data.AccusationRightId == AccusationRightId
                          select data.AccusedId).FirstOrDefault();
            int AccusedId = Convert.ToInt32(result);
            TcommentSection q = (from n in _context.TcommentSections
                                 where n.CommentId == AccusedId
                                 select n).FirstOrDefault();
            q.DisVisible = DisVisible;
            _context.SaveChanges();
            Accusation_progress(AccusationRightId);
        }
        public void Accusation_progress(int AccusationRightId)
        {
            Taccusation _taccusation = (from n in _context.Taccusations
                                        where n.AccusationRightId == AccusationRightId
                                        select n).FirstOrDefault();
            _taccusation.ProgressId = 4;
            _context.SaveChanges();
        }
    }
}
