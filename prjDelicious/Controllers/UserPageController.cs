using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using prjDelicious.Models;
using prjDelicious.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.Controllers
{
    public class UserPageController : Controller 
    {
        private readonly DeliciousContext _deliciousContext ;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IOptions<CEmailSetting> _CEmailSetting;
        private readonly IOptions<CSrcSetting> _CSrcSetting;

        public UserPageController(DeliciousContext deliciousContext,IHostingEnvironment hostingEnvironment, IOptions<CEmailSetting> CEmailSetting, IOptions<CSrcSetting> CSrcSetting)
        {
            _deliciousContext = deliciousContext;
            _hostingEnvironment = hostingEnvironment;
            _CEmailSetting = CEmailSetting;
            _CSrcSetting = CSrcSetting;
        }
        //個人頁面===================================================================
        public IActionResult Profile(int id)
        {
            CUserPageViewModel userPageViewModel = new CUserPageViewModel(id, _deliciousContext);
            userPageViewModel.figureSrc = _CSrcSetting.Value.FigureSrc;
            userPageViewModel.follows = _deliciousContext.TmemberFollows.Where(t => t.MemberId == id).Select(t=>t).ToList();//追蹤了誰
            userPageViewModel.followeds = _deliciousContext.TmemberFollows.Where(t => t.FollowMemberId == id).Select(t=>t).ToList();//被誰追蹤
            return View(userPageViewModel);
        }
        [HttpPost]
        public JsonResult GetMemberRecipe(int id)
        {
            var recipe = from r in _deliciousContext.Trecipes
                         let like = (from r2 in _deliciousContext.TlikeRecipes.AsEnumerable() where r2.RecipeId == r.RecipeId select r2).Count()
                         where r.Member.MemberId == id && r.DisVisible == false && r.DeleteOrNot == false
                         select new
                         {
                             r.RecipeId,
                             r.RecipeName,
                             RecipeDescription = r.RecipeDescription,
                             r.Picture,
                             r.Member.Nickname,
                             r.Views,
                             r.PostTime,
                             LikeCount = like
                         };
            return Json(recipe);
        }
        //修改資料===================================================================
        public IActionResult Setting(int id)
        {
            int? member_id = Convert.ToInt32(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
            if (member_id != 0)
            {
                Tmember member = _deliciousContext.Tmembers.Where(m => m.MemberId == id).FirstOrDefault();
                CUserSettingViewModel model = new CUserSettingViewModel();
                model._member = member;
                model.FigureSrc = _CSrcSetting.Value.FigureSrc;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "HomePage");
            }
        }
        [HttpPost]
        public bool Setting(CUserSettingViewModel model)
        {
            if (model.UpdateMemberSuccess(model, _deliciousContext, _hostingEnvironment))
            {
                HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERNICKNAME, model.Nickname);
                HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERID, model.MemberId.ToString());
                if (model.photo != null)
                {
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERPHOTO, _CSrcSetting.Value.FigureSrc+model._member.Figure);
                }
                return true;
            }
            else 
            {
                return false;
            }
        }
        //修改密碼===================================================================
        public IActionResult ChangePassword(int id)
        {
            int? member_id = Convert.ToInt32(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
            if (member_id != null)
            {
                Tmember member = _deliciousContext.Tmembers.Where(m => m.MemberId == id).FirstOrDefault();
                CChangePasswordViewModel model = new CChangePasswordViewModel()
                {
                    MemberId = member.MemberId,
                    FirstSetPassword = member.FirstSetPassword
                };
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "HomePage");
            }
        }
        [HttpPost]
        public string ChangePassword(CChangePasswordViewModel model)
        {
            var member = _deliciousContext.Tmembers.FirstOrDefault(t => t.MemberId == model.MemberId);
            if (member.FirstSetPassword)
            {
                return model.SetPassword(model, _deliciousContext);
            }
            else
            {
                return model.UpdatePassword(model, _deliciousContext);
            }
        }
        //忘記密碼===================================================================
        public IActionResult ForgetPassword()
        {
            int? member_id = Convert.ToInt32(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
            if (member_id != null)
            {
                Tmember member = _deliciousContext.Tmembers.Where(m => m.MemberId == member_id).FirstOrDefault();
                return View(member);
            }
            else
            {
                return View();
            }
        }
        public string SendPassword(string who)
        {
            var member = _deliciousContext.Tmembers.Where(m => m.AccountName == who || m.Email == who).FirstOrDefault();
            if (member != null)
            {
                CEmailSetting c = new CEmailSetting()
                {
                    MailPort = _CEmailSetting.Value.MailPort,
                    MailServer = _CEmailSetting.Value.MailServer,
                    Password = _CEmailSetting.Value.Password,
                    Sender = _CEmailSetting.Value.Sender,
                    SenderName = _CEmailSetting.Value.SenderName,

                    subject = "瘋廚網重設密碼認證信"
                };
                if (c.MailForgetPassword(member.Email, member.AccountName, member.MemberId))
                {
                    return "登入連結已傳送至您的信箱";
                }
                else 
                {
                    return "信件寄送失敗,請聯絡管理員";
                }
            }
            else
            {
                return "無此信箱或帳號,請重新輸入";
            }
        }
        //重設密碼===================================================================
        public IActionResult ResetPassword(int id)
        {
            var member = _deliciousContext.Tmembers.Where(m => m.MemberId == id).FirstOrDefault();
            CResetPasswordViewModel model = new CResetPasswordViewModel();
            model.MemberId = member.MemberId;
            return View(model);
        }
        [HttpPost]
        public bool ResetPassword(CResetPasswordViewModel model)
        {
            return model.ResetPassword(model, _deliciousContext);
        }
        //信箱驗證================================================================
        public IActionResult EmailConfirm(int id)
        {
            var member = _deliciousContext.Tmembers.Where(m => m.MemberId == id).FirstOrDefault();
            CEmailConfirmViewModel model = new CEmailConfirmViewModel()
            {
                MemberId = member.MemberId,
            };
            return View(model);
        }
        [HttpPost]
        public bool EmailConfirm(CEmailConfirmViewModel model)
        {
            return model.EmailConfirmCheck(model, _deliciousContext);
        }
        [HttpPost]
        public bool SendEmailConfirm(int id)
        {
            var member = _deliciousContext.Tmembers.Where(m=>m.MemberId == id).FirstOrDefault();

            CEmailSetting c = new CEmailSetting()
            {
                //把注入完的資訊丟到類別裡面
                MailPort = _CEmailSetting.Value.MailPort,
                MailServer = _CEmailSetting.Value.MailServer,
                Password = _CEmailSetting.Value.Password,
                Sender = _CEmailSetting.Value.Sender,
                SenderName = _CEmailSetting.Value.SenderName,

                subject = "瘋廚網Email驗證信"
            };
            return c.MailEmailConfirm(member.Email, member.AccountName, member.EmailConfirm);
        }
        //追蹤功能
        [HttpPost]
        public bool AddFollow(int memberID, int followMemberID)
        {
            try
            {
                TmemberFollow new_follow = new TmemberFollow()
                {
                    MemberId = memberID,
                    FollowMemberId = followMemberID
                };
                var isfollow = _deliciousContext.TmemberFollows.FirstOrDefault(t => t.MemberId == memberID && t.FollowMemberId == followMemberID);
                if (isfollow == null)//確定沒有追縱過再加入
                {
                    _deliciousContext.TmemberFollows.Add(new_follow);
                    _deliciousContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        [HttpPost]
        public bool DisFollow(int memberID, int followMemberID)
        {
            try
            {
                var follow = _deliciousContext.TmemberFollows.FirstOrDefault(t => t.MemberId == memberID && t.FollowMemberId == followMemberID);
                if (follow != null)
                {
                    _deliciousContext.TmemberFollows.Remove(follow);
                    _deliciousContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        [HttpPost]
        public JsonResult FollowModal(int memberID)
        {
            var follows = _deliciousContext.TmemberFollows.Where(t => t.MemberId == memberID).Select(t=>new 
            { 
                t.FollowMemberId,
                t.FollowMember.Nickname,
                figure = _CSrcSetting.Value.FigureSrc+t.FollowMember.Figure
            });//追蹤了誰

            return Json(follows);
        }
        [HttpPost]
        public JsonResult FollowedModal(int memberID)
        {
            var followeds = _deliciousContext.TmemberFollows.Where(t => t.FollowMemberId == memberID).Select(t => new 
            { 
                t.MemberId,
                t.Member.Nickname,
                figure = _CSrcSetting.Value.FigureSrc+t.Member.Figure
            });//被誰追蹤

            return Json(followeds);
        }
    }
}
