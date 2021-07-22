using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace prjDelicious.ViewModel
{
    public class CLoginViewModel
    {
        private Tmember loginMember = null;

        public Tmember member { get { return loginMember; } set { loginMember = value; } }
        public CLoginViewModel()
        {
            loginMember = new Tmember(); 
        }

        [DisplayName("註冊帳號/信箱/手機號碼")]
        [Required(ErrorMessage = "請輸入帳號/信箱/手機號碼")]
        public string AccountOrEmailOrCell { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        [DisplayName("密碼")]
        public string Password { get; set; }

        //[Required(ErrorMessage = "請輸入認證碼")]
        //[DisplayName("認證碼")]
        //public string txtCode { get; set; }

        public bool LoginSuccess(CLoginViewModel model,DeliciousContext db)
        {
            string password = model.Password;
            for (int i = 0; i < 100; i++)
            {
                password = Csha256.ConvertToSha256(password);
            }
            var member = db.Tmembers.FirstOrDefault(
                m => (m.AccountName == model.AccountOrEmailOrCell||m.Email == model.AccountOrEmailOrCell||m.CellNumber == model.AccountOrEmailOrCell) &&
                m.Password == password);

            if (member != null)
            {
                this.member = member;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
