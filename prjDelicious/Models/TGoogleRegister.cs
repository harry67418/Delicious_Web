using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjDelicious.Models
{
    public class TGoogleRegister
    {
        public string user_id{ get; set; }
        public string user_email { get; set; }
        public string user_picture { get; set; }
        public string user_account { get { return SplitUserEmail(user_email); } }

        public string SplitUserEmail(string email)
        {
            return email.Substring(0, email.IndexOf('@'));
        }
    }
}
