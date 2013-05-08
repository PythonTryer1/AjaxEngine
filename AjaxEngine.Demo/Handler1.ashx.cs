using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AjaxEngine;
using AjaxEngine.AjaxHandlers;
using System.Drawing;
using AjaxEngine.Utils;

namespace AjaxEngine.Demo
{
    public class T
    {
        public int t1 { get; set; }
        public int t2 { get; set; }
        public T() {
            this.t1 = 1;
            this.t2 = 2;
        }
    }
    /// <summary>
    /// Handler1 的摘要说明
    /// </summary>
    /// 
    [Summary()]
    public class Handler1 : AjaxHandlerBase
    {

        protected override bool PreInvoke()
        {
            this.ThrowError = false;
            return base.PreInvoke();
        }
        [Summary(Description = "这是一个添加方法",
                 Return = "你好中国",
                 Parameters = "t:我是参数")]
        [AjaxMethod(HttpMethod="GET")]
        public T add(T t)
        {
            t.t1++;
            t.t2++;
            return t;
        }
        [AjaxMethod]
        public Bitmap getImage(int id)
        {
            return new Bitmap(100, 100);
        }
    }
}