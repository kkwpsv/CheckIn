using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CheckIn.Common.Util
{
    public static class MD5Helper
    {
        public static string MD5Lower(string str)
        {
            using (IncrementalHash hasher = IncrementalHash.CreateHash(HashAlgorithmName.MD5))
            {
                hasher.AppendData(Encoding.UTF8.GetBytes(str));
                byte[] hash = hasher.GetHashAndReset();
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        public static string PasswordMD5(string pass)
        {
            return MD5Lower(MD5Lower(pass) + "saltforcheckinsystemstorepasswordinserverdatabase");
        }
    }
}
