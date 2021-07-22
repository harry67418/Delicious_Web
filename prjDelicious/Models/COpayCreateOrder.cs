using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace prjDelicious.Models
{
    public class COpayCreateOrder
    {
        public string MerchantID { get; set; } //測試用商家ID：2000132
        //商店交易編號
        public string MerchantTradeNo { get; set; }//用日期時間毫秒來產生不重複的編號
        public string MerchantTradeDate { get; set; } //DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        public string PaymentType { get; set; } //固定使用"aio"
        public int TotalAmount { get; set; }
        public string TradeDesc { get; set; }
        public string ItemName { get; set; }
        public string ReturnURL { get; set; } //付款完成通知回傳網址
        public string ChoosePayment { get; set; }
        public string StoreID { get; set; }
        public string ChooseSubPayment { get; set; } 
        public string ClientBackURL { get; set; } //完成訂單確認後要導回到的專題網站網址
        public int EncryptType { get; set; } //固定使用1
        private string _checkMacValue = "";
        public string CheckMacValue { get { return this.generateMacValue(); } }

        private string generateMacValue() //sha256加密
        {
            var prop_value = new Dictionary<string, string>();
            var sortedKey = new List<string>();
            var props = this.GetType().GetProperties();//取得這個class有幾個屬性
            string data = "";
            foreach (var prop in props)
            {
                if (prop.Name != "CheckMacValue")
                {
                    if (prop.GetValue(this) == null)//如果這個參數沒有設定
                    {
                        prop_value[prop.Name] = "";//value就不用設
                    }
                    else
                    {
                        prop_value[prop.Name] = prop.GetValue(this).ToString();
                    }
                    sortedKey.Add(prop.Name);
                }
            }
            sortedKey.Sort();
            data += "HashKey=5294y06JbISpM5x9";
            foreach (var key in sortedKey)
            {
                data += "&";
                data += key;
                data += "=";
                data += prop_value[key];
            }
            data += "&HashIV=v77hoKGq4kWxNNIS";
            data = HttpUtility.UrlEncode(data).ToLower();
            this._checkMacValue = sha256_hash(data).ToUpper();
            return this._checkMacValue;
        }
        public static String sha256_hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}
