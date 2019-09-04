using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace aspnetmvc_starter.Web.Helpers
{
    public class Encrypt
    {
        public static string GetMD5HASH(string input)
        {
            using (MD5CryptoServiceProvider md = new MD5CryptoServiceProvider())
            {
                byte[] b = System.Text.Encoding.UTF8.GetBytes(input);
                b = md.ComputeHash(b);
                System.Text.StringBuilder sb = new StringBuilder();
                foreach (byte x in b)
                {
                    sb.Append(x.ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}