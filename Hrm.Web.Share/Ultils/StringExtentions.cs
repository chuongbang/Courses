using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Course.Web.Share.Ultils
{
    public static class StringExtentions
    {
        public static string GetCaptchaWord(int length)
        {
            Random random = new Random(System.DateTime.Now.Millisecond);

            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789";
            string cw = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return cw;
        }
    }
}
