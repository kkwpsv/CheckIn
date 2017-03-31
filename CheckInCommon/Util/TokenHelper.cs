using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace CheckIn.Common.Util
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
            var deviceidmd5 = MD5Helper.MD5Lower(deviceid);

            for (int i = 1; i < 16; i += 2)
            {
                token[i] = deviceidmd5[i * 2];
            }

            return new string(token);

        }


    }
}
