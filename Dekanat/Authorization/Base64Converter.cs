using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekanat.Authorization
{
    class Base64Converter
    {
        public static string ToBase64(string text)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }

        public static string FromBase64(string base64text)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(base64text));
        }
    }
}
