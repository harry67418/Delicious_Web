using Microsoft.AspNetCore.Mvc;
using prjDelicious.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace prjDelicious.ViewModel
{
    public class CCreateMemberViewModel
    {
        public Tmember _member { get; set; }

        public CCreateMemberViewModel()
        {
            _member = new Tmember();
        }

        [DisplayName("帳號")]
        [Required(ErrorMessage = "帳號不可為空白")]
        [StringLength(16, ErrorMessage = "請輸入8~16個中英數字", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*[0-9])((?!\s).)+$", ErrorMessage = "帳號須包含英文字母、數字,不可有空白")]
        [Remote(action: "VerifyAccount", controller: "MemberValidation")]
        public string AccountName 
        {
            get { return _member.AccountName; }
            set { _member.AccountName = value; }
        }
        
        [DisplayName("密碼")]
        [Required(ErrorMessage = "密碼不可為空白")]
        [Compare("Password2", ErrorMessage = "兩次輸入密碼必須相同")]
        [StringLength(16, ErrorMessage = "請輸入8~16個中英數字", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*[0-9])((?!\s).)+$", ErrorMessage = "密碼須包含英文字母、數字,不可有空白")]
        public string Password1 
        { 
            get { return _member.Password; }
            set { _member.Password = value; }
        }

        [DisplayName("確認密碼")]
        [Required(ErrorMessage = "密碼不可為空白")]
        [Compare("Password1", ErrorMessage = "兩次輸入密碼必須相同")]
        public string Password2 { get; set; }

        [DisplayName("手機號碼")]
        [Required(ErrorMessage = "手機號碼不可為空白")]
        [StringLength(10, ErrorMessage = "請輸入正確手機號碼", MinimumLength = 10)]
        [RegularExpression(@"^09[0-9]{8}$", ErrorMessage = "請輸入正確手機格式")]
        public string CellNumber
        {
            get { return _member.CellNumber; }
            set { _member.CellNumber = value; }
        }

        [DisplayName("電子信箱")]
        [Required(ErrorMessage = "電子信箱不可為空白")]
        [RegularExpression(@"^([A-Za-z0-9_\-\.\u4e00-\u9fa5])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,8})$", ErrorMessage = "請輸入正確信箱格式")]
        [Remote(action: "VerifyEmail", controller: "MemberValidation")]
        public string Email
        {
            get { return _member.Email; }
            set { _member.Email = value; }
        }

        public int MemberId
        {
            get { return _member.MemberId; }
            set { _member.MemberId = value; }
        }

        public string Password
        {
            get {return _member.Password; }
            set {_member.Password = value; }
        }

        public string Nickname
        {
            get { return _member.Nickname; }
            set { _member.Nickname = _member.AccountName; }
        }
        public string MemberName
        {
            get { return _member.MemberName; }
            set { _member.MemberName = _member.AccountName; }
        }

        public string CellConfirm
        {
            get { return _member.CellConfirm; }
            set { _member.CellConfirm = value; }
        }

        public string EmailConfirm
        {
            get { return _member.EmailConfirm; }
            set { _member.EmailConfirm = value; }
        }

        public string Info
        {
            get { return _member.Info; }
            set { _member.Info = value; }
        }
        public bool ConfirmedOrNotEmail
        {
            get { return _member.ConfirmedOrNotEmail; }
            set { _member.ConfirmedOrNotEmail = value; }
        }
        public bool ConfirmedOrNotPhone
        {
            get { return _member.ConfirmedOrNotPhone; }
            set { _member.ConfirmedOrNotPhone = value; }
        }

        public DateTime RegisterTime
        {
            get { return _member.RegisterTime; }
            set { _member.RegisterTime = value; }
        }
        public int PersonalRankScore
        {
            get { return _member.PersonalRankScore; }
            set { _member.PersonalRankScore = value; }
        }
        public int RankId
        {
            get { return _member.RankId; }
            set { _member.RankId = value; }
        }
        public string Figure
        {
            get { return _member.Figure; }
            set { _member.Figure = value; }
        }
        public bool FirstSetPassword
        {
            get {return _member.FirstSetPassword; }
            set { _member.FirstSetPassword = value;}
        }
        public void FillMemberData()
        {
            string CellConfirm = "";
            string EmailConfirm = "";
            Random r = new Random();
            Random r2 = new Random();
            for (int i = 0; i < 6; i++)
            {
                CellConfirm += r.Next(0, 9).ToString();
                EmailConfirm += r2.Next(0, 9).ToString();
            }
            this.Nickname = _member.AccountName;
            this.MemberName = _member.AccountName;
            this.CellConfirm = CellConfirm;
            this.EmailConfirm = EmailConfirm;
            this.Info = "尚未編輯個人簡介";
            this.ConfirmedOrNotEmail = false;
            this.ConfirmedOrNotPhone = false;
            this.Figure = "default.jpg";
            this.RegisterTime = DateTime.Now.ToLocalTime();
            this.PersonalRankScore = 0;
            this.RankId = 1;
            this.FirstSetPassword = false;
            for (int i = 0; i < 100; i++)
            {
                this.Password1 = Csha256.ConvertToSha256(this.Password1);
            }
            this.Password = this.Password1;

        }
        public bool AddMember(Tmember member,DeliciousContext deliciousContext)
        {
            try
            {
                deliciousContext.Add(member);
                deliciousContext.SaveChanges();
                int newMemberId = deliciousContext.Tmembers.FirstOrDefault(t => t.AccountName == member.AccountName).MemberId;
                TcollectionFolder tcollection = new TcollectionFolder()
                {
                    MemberId = newMemberId,
                    CollectionFolder = "我的最愛",
                };
                deliciousContext.Add(tcollection);
                deliciousContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
