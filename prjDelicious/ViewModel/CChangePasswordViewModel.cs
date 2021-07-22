using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class CChangePasswordViewModel
    {
        public CChangePasswordViewModel()
        {

        }
        public int MemberId { get; set; }
        public bool FirstSetPassword { get; set; }

        [DisplayName("輸入舊密碼")]
        [Required(ErrorMessage = "請輸入舊密碼")]
        public string password { get; set; }

        [DisplayName("輸入新密碼")]
        [Required(ErrorMessage = "密碼不可為空白")]
        [Compare("newPassword2", ErrorMessage = "兩次輸入密碼必須相同")]
        [StringLength(16, ErrorMessage = "請輸入8~16個中英數字", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[0-9])((?!\s).)+$", ErrorMessage = "請輸入8~16個中英數字")]
        public string newPassword { get; set; }
        [DisplayName("確認新密碼")]
        [Required(ErrorMessage = "密碼不可為空白")]
        [Compare("newPassword", ErrorMessage = "兩次輸入密碼必須相同")]
        public string newPassword2 { get; set; }

        public string UpdatePassword(CChangePasswordViewModel model, DeliciousContext _deliciousContext)
        {
            for (int i = 0; i < 100; i++)
            {
                model.password = Csha256.ConvertToSha256(model.password);
                model.newPassword = Csha256.ConvertToSha256(model.newPassword);
            }

            var member = _deliciousContext.Tmembers.Where(m => m.MemberId == model.MemberId).FirstOrDefault();
            if (member != null)
            {
                if (member.Password != model.password)
                {
                    return "舊密碼錯誤";
                }
                else
                {
                    member.Password = model.newPassword;
                    try
                    {
                        _deliciousContext.SaveChanges();
                        return "修改密碼成功";
                    }
                    catch (Exception ex)
                    {
                        return "修改密碼失敗";
                    }
                }
            }
            else
            {
                return "修改密碼失敗";
            }
        }
        public string SetPassword(CChangePasswordViewModel model, DeliciousContext _deliciousContext)
        {
            for (int i = 0; i < 100; i++)
            {
                model.newPassword = Csha256.ConvertToSha256(model.newPassword);
            }
            var member = _deliciousContext.Tmembers.Where(m => m.MemberId == model.MemberId).FirstOrDefault();
            if (member != null)
            {
                member.Password = model.newPassword;
                member.FirstSetPassword = false;
                try
                {
                    _deliciousContext.SaveChanges();
                    return "修改密碼成功";
                }
                catch (Exception ex)
                {
                    return "修改密碼失敗";
                }
            }
            else
            {
                return "修改密碼失敗";
            }
        }
    }
}
