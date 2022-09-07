using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace OrgProject
{

    // to encrpt & decrpt the "id" input hidden in view

    public class EncryptDecrypt
    {
        public static string Encode(string encodeMe)
        {
            byte[] encoded = Encoding.UTF8.GetBytes(encodeMe);
            return Convert.ToBase64String(encoded);
        }

        public static string Decode(string decodeMe)
        {
            byte[] encoded = Convert.FromBase64String(decodeMe);
            return Encoding.UTF8.GetString(encoded);
        }

    }
}