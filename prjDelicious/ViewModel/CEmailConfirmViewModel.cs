using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class CEmailConfirmViewModel
    {
        Tmember _member { get; set; }

        public CEmailConfirmViewModel()
        {
            _member = new Tmember();
        }

        public int MemberId { get; set; }


        [DisplayName("輸入驗證碼")]
        [Required(ErrorMessage = "請輸入驗證碼")]
        public string EmailConfirm { get; set; }

        public bool EmailConfirmCheck(CEmailConfirmViewModel model , DeliciousContext _deliciousContext)
        {
            try
            {
                var member = _deliciousContext.Tmembers.Where(m => m.MemberId == model.MemberId).FirstOrDefault();
                if (model.EmailConfirm == member.EmailConfirm)
                {
                    member.ConfirmedOrNotEmail = true;
                    string newConfirm = "";
                    Random r = new Random();
                    for (int i = 0; i < 6; i++)
                    {
                        newConfirm += r.Next(0, 9).ToString();
                    }
                    member.EmailConfirm = newConfirm;
                    _deliciousContext.SaveChanges();
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }

        }
    }
}
