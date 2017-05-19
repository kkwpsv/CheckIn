using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.API
{
    public class CheckCodeHelper
    {
        public static (MemoryStream image, string text) GetCheckCode()
        {

            string[] chars = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,P,P,Q,R,S,T,U,V,W,X,Y,Z".Split(',');
            string code = "";

            Random rand = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < 4; i++)
            {
                var t = rand.Next(chars.Length);//获取随机数  
                code += chars[t];//随机数的位数加一  
            }


            Bitmap Img = null;
            Graphics g = null;
            MemoryStream ms = null;
            Color[] colors = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            string[] fonts = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
            Img = new Bitmap(72, 32);
            g = Graphics.FromImage(Img);
            g.Clear(Color.White);
            for (int i = 0; i < 100; i++)
            {
                int x = rand.Next(Img.Width);
                int y = rand.Next(Img.Height);
                g.DrawRectangle(new Pen(Color.LightGray, 0), x, y, 1, 1);
            }
            for (int i = 0; i < code.Length; i++)
            {
                int cindex = rand.Next(colors.Length);
                int findex = rand.Next(fonts.Length);
                Font font = new Font(fonts[findex], 15, FontStyle.Bold);
                Brush brush = new SolidBrush(colors[cindex]);
                int ii = 4;
                if ((i + 1) % 2 == 0)
                {
                    ii = 2;
                }
                g.DrawString(code.Substring(i, 1), font, brush, 3 + (i * 12), ii);
            }
            ms = new MemoryStream();
            Img.Save(ms, ImageFormat.Jpeg);
            ms.Flush();
            g.Dispose();
            Img.Dispose();
            return (ms, code);
        }
    }
}
