using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace CheckIn.API
{
    public static class TokenHelper
    {
        public static string BuildToken(string deviceid)
        {
            char[] token = new char[16];

            var avaliable = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var random = new Random(Environment.TickCount);

            for (int i = 0; i < 16; i += 2)
            {
                var value = avaliable[random.Next(0, avaliable.Length)];
                token[i] = value;
            }
            var deviceidmd5 = MD5(deviceid);

            for (int i = 1; i < 16; i += 2)
            {
                token[i] = deviceidmd5[i * 2];
            }

            return new string(token);

        }
        private static string MD5(string str)
        {
            using (IncrementalHash hasher = IncrementalHash.CreateHash(HashAlgorithmName.MD5))
            {
                hasher.AppendData(Encoding.Unicode.GetBytes(str));
                byte[] hash = hasher.GetHashAndReset();
                return BitConverter.ToString(hash).Replace("-", "");
            }

        }

    }
}
