using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.Models
{
    public class CEmailSetting
    {
        public CEmailSetting()
        {
        }
        public int MailPort { get; set; }
        public string MailServer { get; set; }
        public string Password { get; set; }
        public string Sender { get; set; }
        public string SenderName { get; set; }

        public string subject { get; set; }

        public bool MailForgetPassword(string email,string name,int id)
        {
            try
            {
                MimeMessage message = new MimeMessage();

                // 添加寄件者
                message.From.Add(new MailboxAddress(SenderName, Sender));
                // 添加收件者
                message.To.Add(new MailboxAddress(name, email));
                // 設定郵件標題
                message.Subject = subject;

                // 使用 BodyBuilder 建立郵件內容
                BodyBuilder bodyBuilder = new BodyBuilder();
                // 設定 HTML 內容
                string url = "https://deliciousnet.azurewebsites.net/UserPage/ResetPassword?id=" + id;
                bodyBuilder.HtmlBody = string.Format(@"<h1>以{0}身分登入</h1><a href=""{1}"">點擊登入</a>", name, url);
                // 設定附件
                //bodyBuilder.Attachments.Add("檔案路徑");
                // 設定郵件內容
                message.Body = bodyBuilder.ToMessageBody();

                using (SmtpClient client = new SmtpClient())
                {
                    client.Connect(MailServer,MailPort,
                        SecureSocketOptions.StartTls);
                    client.Authenticate(Sender, Password);
                    client.Send(message);
                    client.Disconnect(true);
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public bool MailEmailConfirm(string email, string name, string confirm)
        {
            try
            {
                MimeMessage message = new MimeMessage();

                // 添加寄件者
                message.From.Add(new MailboxAddress(SenderName, Sender));
                // 添加收件者
                message.To.Add(new MailboxAddress(name, email));
                // 設定郵件標題
                message.Subject = subject;

                // 使用 BodyBuilder 建立郵件內容
                BodyBuilder bodyBuilder = new BodyBuilder();
                // 設定 HTML 內容
                bodyBuilder.HtmlBody = string.Format(@"<h1>瘋廚網email認證信</h1><span>您的認證碼為:</span><span>{0}</span>", confirm);
                // 設定附件
                //bodyBuilder.Attachments.Add("檔案路徑");
                // 設定郵件內容
                message.Body = bodyBuilder.ToMessageBody();

                using (SmtpClient client = new SmtpClient())
                {
                    client.Connect(MailServer, MailPort,
                        SecureSocketOptions.StartTls);
                    client.Authenticate(Sender, Password);
                    client.Send(message);
                    client.Disconnect(true);
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public void MailNewRecipeNotification(int recipeid,string author,List<TmemberFollow> follows)
        {
            InternetAddressList list = new InternetAddressList();
            foreach (var follow in follows)
            {
                list.Add(new MailboxAddress(follow.Member.Email));
            }
            try
            {
                MimeMessage message = new MimeMessage();
                // 添加寄件者
                message.From.Add(new MailboxAddress(SenderName, Sender));
                //寄給很多人
                message.To.AddRange(list);
                // 設定郵件標題
                message.Subject = subject;
                // 使用 BodyBuilder 建立郵件內容
                BodyBuilder bodyBuilder = new BodyBuilder();
                // 設定 HTML 內容
                string recipeUrl = "https://deliciousnet.azurewebsites.net/ForRecipe/Recipe?id=" + recipeid;
                bodyBuilder.HtmlBody = string.Format(@"<h1>您追蹤的使用者-{0}-發布新的食譜囉!!</h1><a href=""{1}"">點擊前往觀看</a>",author, recipeUrl);
                // 設定郵件內容
                message.Body = bodyBuilder.ToMessageBody();
                using (SmtpClient client = new SmtpClient())
                {
                    client.Connect(MailServer, MailPort,
                        SecureSocketOptions.StartTls);
                    client.Authenticate(Sender, Password);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
