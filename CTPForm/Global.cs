using CTPCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CTPForm
{
    public class Global
    {
        public static FormMain frmMain;

        internal static void CBInfo(DataResult result, string action)
        {
            if (result.IsSuccess)
            {
                Global.AddInfo("{0} success", action);
            }
            else
            {
                Global.GBAddInfo("{0} failed: {1}", action, result.Error);
            }
        }

        public static void AddInfo(string msg, params object[] args)
        {
            if (frmMain!=null)
                frmMain.AddInfo(msg, args);
        }

        public static void GBAddInfo(string msg, params object[] args)
        {
            msg = string.Format(msg, args);
            msg = GbText(msg);
            AddInfo(msg);
        }

        public static string GbText(string text)
        {
            byte[] gb = Encoding.Default.GetBytes(text);
            return Encoding.GetEncoding("gb2312").GetString(gb);
        }

        public static string Utf2Gb(string text)
        {
            //gb2312   
            Encoding gb2312 = Encoding.GetEncoding("gb2312");
            Encoding utf8 = Encoding.GetEncoding("utf-8");

            byte[] gb;
            gb = utf8.GetBytes(text);
            gb = Encoding.Convert(utf8, gb2312, gb);

            //返回转换后的字符  
            string utext = gb2312.GetString(gb);
            return utext;
        }
    }
}
