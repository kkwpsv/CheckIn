using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CheckIn.Common.Util
{
    public static class HeadImageHelper
    {
        public static byte[] GetHeadImage(int userid)
        {
            EnsureDictionary();

            if (File.Exists($@"HeadImage/{userid}"))
            {
                return File.ReadAllBytes($@"HeadImage/{userid}");
            }
            else
            {
                return null;
            }
        }
        public static void SaveHeadImage(int userid, byte[] data)
        {
            EnsureDictionary();

            File.WriteAllBytes($@"HeadImage/{userid}", data);
        }

        private static void EnsureDictionary()
        {
            if (!Directory.Exists(@"HeadImage"))
            {
                Directory.CreateDirectory(@"HeadImage");
            }
        }
    }
}
