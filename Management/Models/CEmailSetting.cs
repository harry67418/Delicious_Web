using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Models
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

        public bool MailOrderStatus(string Email, string MemberName,string OrderStatus,string PayMethod,string OrderDate)
        {
            try
            {
                MimeMessage message = new MimeMessage();

                // 添加寄件者
                message.From.Add(new MailboxAddress(SenderName, Sender));
                // 添加收件者
                message.To.Add(new MailboxAddress(MemberName, Email));
                // 設定郵件標題
                message.Subject = subject;

                // 使用 BodyBuilder 建立郵件內容
                BodyBuilder bodyBuilder = new BodyBuilder();
                // 設定 HTML 內容
                if (OrderStatus == "出貨中")
                { bodyBuilder.HtmlBody = string.Format(@"<p><h4>親愛的 {0}會員您好</h4></p><p>您於{1}的訂單，已於今日出貨，會於3日工作天內抵達，感謝您的訂購。</p><p>瘋廚網商城 祝您烹飪愉快 </p><p>連絡電話:02 6631 6599</p><p>連絡信箱:delicious13001@outlook.com</p>", MemberName, OrderDate); }
                else if (OrderStatus == "已取消")
                { bodyBuilder.HtmlBody = string.Format(@"<p><h4>親愛的 {0}會員您好</h4></p><p>您於{1}的訂單，已於今日取消，造成您的不便請多多包涵。</p><p>瘋廚網商城 祝您事事順心 </p><p>連絡電話:02 6631 6599</p><p>連絡信箱:delicious13001@outlook.com</p>", MemberName, OrderDate); }
                // 設定附件
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
        //public bool MailEmailConfirm(string email, string name, string confirm)
        //{
        //    try
        //    {
        //        MimeMessage message = new MimeMessage();

        //        // 添加寄件者
        //        message.From.Add(new MailboxAddress(SenderName, Sender));
        //        // 添加收件者
        //        message.To.Add(new MailboxAddress(name, email));
        //        // 設定郵件標題
        //        message.Subject = subject;

        //        // 使用 BodyBuilder 建立郵件內容
        //        BodyBuilder bodyBuilder = new BodyBuilder();
        //        // 設定 HTML 內容
        //        bodyBuilder.HtmlBody = string.Format(@"<h1>瘋廚網email認證信</h1><span>您的認證碼為:</span><span>{0}</span>", confirm);
        //        // 設定附件
        //        //bodyBuilder.Attachments.Add("檔案路徑");
        //        // 設定郵件內容
        //        message.Body = bodyBuilder.ToMessageBody();

        //        using (SmtpClient client = new SmtpClient())
        //        {
        //            client.Connect(MailServer, MailPort,
        //                SecureSocketOptions.StartTls);
        //            client.Authenticate(Sender, Password);
        //            client.Send(message);
        //            client.Disconnect(true);
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {

        //        return false;
        //    }
        //}
    }
}
