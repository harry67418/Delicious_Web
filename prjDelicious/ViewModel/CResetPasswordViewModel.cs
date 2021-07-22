using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class CResetPasswordViewModel
    {
        public CResetPasswordViewModel()
        {

        }

        public int MemberId { get; set; }

        [DisplayName("輸入新密碼")]
        [Required(ErrorMessage = "密碼不可為空白")]
        [Compare("Password2", ErrorMessage = "兩次輸入密碼必須相同")]
        [StringLength(16, ErrorMessage = "請輸入8~16個中英數字", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[0-9]).+$", ErrorMessage = "請輸入8~16個中英數字")]
        public string Password1 { get; set; }

        [DisplayName("再次輸入新密碼新密碼")]
        [Required(ErrorMessage = "密碼不可為空白")]
        [Compare("Password1", ErrorMessage = "兩次輸入密碼必須相同")]
        public string Password2 { get; set; }


        public bool ResetPassword(CResetPasswordViewModel model, DeliciousContext _deliciousContext)
        {
            var member = _deliciousContext.Tmembers.Where(m => m.MemberId == model.MemberId).FirstOrDefault();
            for (int i = 0; i < 100; i++)
            {
                model.Password1 = Csha256.ConvertToSha256(model.Password1);
            }
            if (member != null)
            {
                member.Password = model.Password1;
                _deliciousContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
