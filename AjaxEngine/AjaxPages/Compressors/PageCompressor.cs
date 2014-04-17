using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace AjaxEngine
{
    public class PageCompressor
    {
        /// <summary>
        /// 指定编码方式是否是浏览器支持的格式
        /// </summary>
        /// <param name="encoding">编码名称</param>
        /// <returns>布尔值</returns>
        private bool IsEncodingAccepted(string encoding)
        {
            HttpContext context = HttpContext.Current;
            return context.Request.Headers["Accept-encoding"] == null ||
                          context.Request.Headers["Accept-encoding"].ToLower().Contains(encoding) ||
                          context.Request.Headers["Accept-encoding"].ToLower().Contains("*");
        }
        /// <summary>
        /// 设置页面编码方式
        /// </summary>
        /// <param name="encoding">编码名称</param>
        public void SetEncoding(string encoding)
        {
            HttpContext.Current.Response.AppendHeader("Content-encoding", encoding);
        }
        /// <summary>
        /// 页面压缩类型（更改压缩类型请重载此属性）
        /// </summary>
        public virtual PageCompressType PageCompressType
        {
            get
            {
                return PageCompressType.GZip;
            }
        }
        /// <summary>
        /// 页面压缩处理，此方法在Render中被调用
        /// </summary>
        public void PageCompressHandle()
        {
            PageCompressType ct = this.PageCompressType;
            if (ct != PageCompressType.None && IsEncodingAccepted(ct.ToString().ToLower()))
            {
                if (ct == PageCompressType.GZip)
                {
                    GZipStream zs = new GZipStream(HttpContext.Current.Response.Filter, CompressionMode.Compress);
                    HttpContext.Current.Response.Filter = zs;
                }
                else if (ct == PageCompressType.Defalte)
                {
                    DeflateStream ds = new DeflateStream(HttpContext.Current.Response.Filter, CompressionMode.Compress);
                    HttpContext.Current.Response.Filter = ds;
                }
                SetEncoding(ct.ToString().ToLower());
            }
        }
    }
}
