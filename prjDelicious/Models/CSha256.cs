﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace prjDelicious.Models
{
    public static class Csha256
    {
        public static string ConvertToSha256(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hash = SHA256Managed.Create().ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("X2"));
            }

            return builder.ToString();
        }
    }
}
