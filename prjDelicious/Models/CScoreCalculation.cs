using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.Models
{
    public class CScoreCalculation
    {
        private readonly DeliciousContext _deliciousContext;
        public CScoreCalculation(DeliciousContext deliciousContext)
        {
            _deliciousContext = deliciousContext;
        }
        public bool AddComment(int memberid,int score =20)
        {
            var member = _deliciousContext.Tmembers.Where(m => m.MemberId == memberid).FirstOrDefault();
            if (member != null)
            {
                member.PersonalRankScore += score;
                try
                {
                    _deliciousContext.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool DeleteComment(int memberid,int score=20)
        {
            var member = _deliciousContext.Tmembers.Where(m => m.MemberId == memberid).FirstOrDefault();
            if (member != null)
            {
                member.PersonalRankScore -= score;
                try
                {
                    _deliciousContext.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool LikeComment(int memberid , int score = 10)
        {
            var member = _deliciousContext.Tmembers.Where(m => m.MemberId == memberid).FirstOrDefault();
            if (member != null)
            {
                member.PersonalRankScore += score;
                try
                {
                    _deliciousContext.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else 
            {
                return false;
            }
        }
        public bool DisLikeComment(int memberid, int score = 10)
        {
            var member = _deliciousContext.Tmembers.Where(m => m.MemberId == memberid).FirstOrDefault();
            if (member != null)
            {
                member.PersonalRankScore -= score;
                try
                {
                    _deliciousContext.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool GetPoint(int memberId, int score = 40)
        {
            var member = _deliciousContext.Tmembers.Where(a => a.MemberId == memberId).FirstOrDefault();
            member.PersonalRankScore += score;

            try
            {
                _deliciousContext.SaveChanges();
                return true;
            }
            catch (Exception error)
            {
                return false;
            }
        }

        public bool LosePoint(int memberId, int score = 40)
        {
            var member = _deliciousContext.Tmembers.Where(a => a.MemberId == memberId).FirstOrDefault();
            member.PersonalRankScore -= score;

            try
            {
                _deliciousContext.SaveChanges();
                return true;
            }
            catch (Exception error)
            {
                return false;
            }
        }
    }
}
