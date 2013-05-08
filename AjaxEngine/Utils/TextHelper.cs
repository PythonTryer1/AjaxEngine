using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AjaxEngine.Utils
{
    public static class TextHelper
    {
        /// <summary>
        /// 过滤字符串，用于向客户端输出
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FilterString(string str)
        {
            if (str != null)
                return str.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n");
            else
                return "";
        }
        /// <summary>
        /// 对应js中的escape
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Escape(string s)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                byte[] ba = System.Text.Encoding.Unicode.GetBytes(s);
                for (int i = 0; i < ba.Length; i += 2)
                {
                    sb.Append("%u");
                    sb.Append(ba[i + 1].ToString("X2"));
                    sb.Append(ba[i].ToString("X2"));
                }
                return sb.ToString();
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>编码字符串</returns>
        public static string Encode(string s)
        {
            return Escape(s);
            //return HttpUtility.UrlEncode(s);
        }
        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="s">编码字符串</param>
        /// <returns>解码字符串</returns>
        public static string Decode(string s)
        {
            return Unescape(s);
        }
        /// <summary>
        /// 对应js中的unescape
        /// </summary>
        /// <param name="s">编码字符串</param>
        /// <returns>解码字符串</returns>
        public static string Unescape(string s)
        {
            try
            {
                return HttpUtility.UrlDecode(s);
            }
            catch
            {
                return null;
            }
        }
    }
}
