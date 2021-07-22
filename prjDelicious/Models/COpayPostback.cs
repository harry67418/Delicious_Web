using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace prjDelicious.Models
{
    public class COpayPostback
    {
        public string MerchantID { get; set; }
        public string MerchantTradeNo { get; set; }
        public int PayAmt { get; set; }
        public string PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public int PaymentTypeChargeFee { get; set; }
        public int RedeemAmt { get; set; }
        public int RtnCode { get; set; }
        public string RtnMsg { get; set; }
        public int SimulatePaid { get; set; }
        public int TradeAmt { get; set; }
        public string TradeDate { get; set; }
        public string TradeNo { get; set; }

        private string _MyCheckMacValue = "";
        public string MyCheckMacValue { get { return this.generateMacValue(); } }

        private string generateMacValue() //sha256加密
        {
            var prop_value = new Dictionary<string, string>();
            var sortedKey = new List<string>();
            var props = this.GetType().GetProperties();//取得這個class有幾個屬性
            string data = "";
            foreach (var prop in props)
            {
                if (prop.Name != "MyCheckMacValue")
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
            this._MyCheckMacValue = sha256_hash(data).ToUpper();
            return this._MyCheckMacValue;
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
