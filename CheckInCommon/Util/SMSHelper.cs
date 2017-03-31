using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CheckIn.Common.Util
{
    public class SMSHelper
    {
        public static async Task<int> SendCheckCode(string phonenumber)
        {
            var client = new HttpClient();
            var ms = new MemoryStream();
            var content = new StreamContent(ms);

            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string sid = "e4fa0ed52b5e4d878d27a759950adbdf";
            string token = "0299bcef26f94b139a54cefad6386561";
            string sig = MD5Helper.MD5Lower(sid + token + timestamp).ToLower();

            int code = new Random().Next(1000, 10000);


            string poststring = $@"accountSid={sid}&smsContent=【掌上签到】您的验证码为{code}，请于3分钟内正确输入，如非本人操作，请忽略此短信。&to={phonenumber}&timestamp={timestamp}&sig={sig}";
            var postdata = Encoding.UTF8.GetBytes(poststring);
            ms.Write(postdata, 0, postdata.Length);
            ms.Seek(0, SeekOrigin.Begin);



            var t = await client.PostAsync("https://api.miaodiyun.com/20150822/industrySMS/sendSMS", content);
            var response = await t.Content.ReadAsStringAsync();
            if (response.StartsWith(@"{""respCode"":""00000"""))
            {

                return code;
            }
            else
            {
                return 0;
            }
        }

    }
}
