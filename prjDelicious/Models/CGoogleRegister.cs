using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace prjDelicious.Models
{
    public class CGoogleRegister
    {
        private readonly DeliciousContext _deliciousContext;
        private readonly IWebHostEnvironment _hostEnvironment;
        private Tmember _member { get; set; }
        private TGoogleRegister _googleRegister { get; set; }
        public CGoogleRegister(TGoogleRegister googleRegister,DeliciousContext deliciousContext, IWebHostEnvironment hostEnvironment)
        {
            _deliciousContext = deliciousContext;
            _member = new Tmember();
            _googleRegister = googleRegister;
            _hostEnvironment = hostEnvironment;
        }
        public Tmember CreateMember()
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

            _member.AccountName = _googleRegister.user_account;
            _member.Nickname = _googleRegister.user_account;
            _member.MemberName = _googleRegister.user_account;
            _member.CellConfirm = CellConfirm;
            _member.Email = _googleRegister.user_email;
            _member.EmailConfirm = EmailConfirm;
            _member.Info = "尚未編輯個人簡介";
            _member.ConfirmedOrNotEmail = true;
            _member.ConfirmedOrNotPhone = false;
            _member.RegisterTime = DateTime.Now.ToLocalTime();
            _member.PersonalRankScore = 0;
            _member.RankId = 1;
            _member.Password = Csha256.ConvertToSha256(_googleRegister.user_id);
            _member.Figure = getImageFromURL(_googleRegister.user_picture);
            _member.GoogleId = _googleRegister.user_id;
            _member.FirstSetPassword = true;
            try
            {
                _deliciousContext.Add(_member);
                _deliciousContext.SaveChanges();
                int newMemberId = _deliciousContext.Tmembers.FirstOrDefault(t => t.AccountName == _member.AccountName).MemberId;
                TcollectionFolder tcollection = new TcollectionFolder()
                {
                    MemberId = newMemberId,
                    CollectionFolder = "我的最愛",
                };
                _deliciousContext.Add(tcollection);
                _deliciousContext.SaveChanges();
                return _member;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public string getImageFromURL(string strUrl)
        {
            Image MyImage = null;

            try
            {
                //建立一個 Web Request
                WebRequest MyWebRequest = WebRequest.Create(strUrl);
                //由 Web Request 取得 Web Response
                WebResponse MyWebResponse = MyWebRequest.GetResponse();
                //由 Web Response 取得 Stream
                Stream MyStream = MyWebResponse.GetResponseStream();
                //由 Stream 取得 Imageㄈ
                MyImage = Image.FromStream(MyStream);

                string filename= DateTime.Now.ToFileTime().ToString() +  ".jpg";
                MyImage.Save(_hostEnvironment.WebRootPath + @"\assets\img\figure\"+filename);

                //該關的關一關, 該放的放一放
                MyStream.Close();
                MyStream.Dispose();
                MyWebResponse.Close();
                MyWebResponse = null;
                MyWebRequest = null;

                return filename;

            }
            catch (Exception)
            {
                throw new Exception("getImageFromURL(string strUrl)發生例外，可能抓不到網路上的圖片" + strUrl);
            }
        }
    }
}
