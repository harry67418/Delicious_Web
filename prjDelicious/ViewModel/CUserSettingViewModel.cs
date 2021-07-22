using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class CUserSettingViewModel
    {
        public Tmember _member { get; set; }

        public CUserSettingViewModel()
        {
            _member = new Tmember();
        }

        public int MemberId 
        { 
            get { return _member.MemberId; }
            set { _member.MemberId = value; } 
        }

        [DisplayName("*暱稱")]
        public string Nickname
        {
            get { return _member.Nickname; }
            set { _member.Nickname = value; }
        }

        [DisplayName("*真實姓名")]
        [Remote(action: "VerifySettingMemberName", controller: "MemberValidation")]
        public string MemberName
        {
            get { return _member.MemberName; }
            set { _member.MemberName = value; }
        }

        [DisplayName("手機號碼")]
        [StringLength(10, ErrorMessage = "請輸入正確手機號碼", MinimumLength = 10)]
        [RegularExpression(@"^09[0-9]{8}$", ErrorMessage = "請輸入正確手機號碼")]
        public string CellNumber
        {
            get { return _member.CellNumber; }
            set { _member.CellNumber = value; }
        }

        [DisplayName("*電子信箱")]
        [RegularExpression(@"^([A-Za-z0-9_\-\.\u4e00-\u9fa5])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,8})$", ErrorMessage = "請輸入正確信箱格式")]
        [Remote(action: "VerifySettingEmail", controller: "MemberValidation")]
        public string Email
        {
            get { return _member.Email; }
            set { _member.Email = value; }
        }

        [DisplayName("個人簡介")]
        public string Info
        {
            get { return _member.Info; }
            set { _member.Info = value; }
        }


        [DisplayName("性別")]
        public string Gender
        {
            get { return _member.Gender; }
            set { _member.Gender = value; }
        }

        [DisplayName("生日")]
        public DateTime? Birthday
        {
            get { return _member.Birthday; }
            set { _member.Birthday = value; }
        }

        [DisplayName("變更大頭貼")]
        public string Figure
        {
            get { return _member.Figure; }
            set { _member.Figure = value; }
        }

        public string FigureSrc { get; set; }

        public IFormFile photo { get; set; }

        public bool UpdateMemberSuccess(CUserSettingViewModel model, DeliciousContext _deliciousContext, IHostingEnvironment _hostingEnvironment)
        {
            var member = _deliciousContext.Tmembers.Where(m => m.MemberId == model.MemberId).FirstOrDefault();//找出這位使用者

            if (member != null)
            {
                member.Nickname = model.Nickname;
                member.MemberName = model.MemberName;
                if (string.IsNullOrWhiteSpace(model.Info)) 
                {
                    model.Info = "尚未編輯個人簡介";
                }
                member.Info = model.Info;
                member.Gender = model.Gender;
                member.Birthday = model.Birthday;
                if (member.CellNumber != model.CellNumber)//如果手機有變,就要重新產生驗證碼
                {
                    member.CellNumber = model.CellNumber;
                    member.ConfirmedOrNotPhone = false;
                    member.CellConfirm = "";
                    Random r = new Random();
                    for (int i = 0; i < 6; i++)
                    {
                        member.CellConfirm += r.Next(0, 9).ToString();
                    }
                }
                if (member.Email != model.Email)//如果email有變,就要重新產生驗證碼
                {
                    member.Email = model.Email;
                    member.ConfirmedOrNotEmail = false;
                    member.EmailConfirm = "";
                    Random r = new Random();
                    for (int i = 0; i < 6; i++)
                    {
                        member.EmailConfirm += r.Next(0, 9).ToString();
                    }
                }
                if (model.photo != null)//如果圖片有更改
                {
                    member.Figure = DateTime.Now.ToFileTime().ToString() + ".jpg";
                    _member.Figure = member.Figure;
                    model.photo.CopyTo(new FileStream(
                        _hostingEnvironment.WebRootPath + @"\assets\img\figure\" + member.Figure,
                        FileMode.Create));
                }
                try
                {
                    _deliciousContext.SaveChanges();
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }
            return false;
        }
    }
}
