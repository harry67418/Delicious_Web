using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjDelicious.Models;
using prjDelicious.ViewModel;

namespace prjDelicious.Controllers
{
    public class MemberValidationController : Controller
    {
        private readonly DeliciousContext _deliciousContext;
        public MemberValidationController(DeliciousContext deliciousContext)
        {
            _deliciousContext = deliciousContext;
        }


        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyAccount(CLoginAndCreateViewModel model)
        {
            var member = _deliciousContext.Tmembers.FirstOrDefault(t => t.AccountName == model.cCreate.AccountName);
            if (member != null)
            {
                return Json("此帳號已被註冊");
            }
            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyEmail(CLoginAndCreateViewModel model)
        {
            var member = _deliciousContext.Tmembers.FirstOrDefault(t => t.Email == model.cCreate.Email);
            if (member != null)
            {
                return Json("此信箱已被註冊");
            }
            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifySettingEmail(string Email)
        {
            var this_member = _deliciousContext.Tmembers.FirstOrDefault(t => t.MemberId.ToString() == HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
            var member = _deliciousContext.Tmembers.FirstOrDefault(t => t.Email == Email);
            if (String.IsNullOrWhiteSpace(Email))
            {
                return Json(true);
            }
            if (member != null)
            {
                if (this_member.Email == member.Email)
                {
                    return Json(true);
                }
                else
                {
                    return Json("此信箱已被註冊");
                }
            }
            else
            {
                return Json(true);
            }
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifySettingMemberName(string MemberName)
        {
            var this_member = _deliciousContext.Tmembers.FirstOrDefault(t => t.MemberId.ToString() == HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
            if (String.IsNullOrWhiteSpace(MemberName))
            {
                return Json(true);
            }
            if (this_member.MemberName == MemberName)
            {
                return Json(true);
            }
            else
            {
                Regex regex = new Regex(@"[\u4E00-\u9FFF]{2,4}");
                if (regex.IsMatch(MemberName))
                {
                    return Json(true);
                }
                else
                {
                    return Json("請輸入正確姓名");
                }
            }
        }
    }
}
