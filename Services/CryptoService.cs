using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CryptoService
    {
        byte[] salt = new byte[128 / 8] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        public static string GetHash(string input)
        {
            var md5 = new MD5CryptoServiceProvider();
            return Encoding.ASCII.GetString(md5.ComputeHash(Encoding.ASCII.GetBytes(input)));
        }

        public static string GetValueFromHash(string hashData)
        {
            return ASCIIEncoding.ASCII.GetString(Encoding.ASCII.GetBytes(hashData));   
        }
    }
}
