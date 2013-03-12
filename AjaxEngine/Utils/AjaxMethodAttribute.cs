/**********************************************************
 * 
 * AjaxAttribute 用于将方法标记为Ajax方法 
 * -------------------------------------------------------
 * 作者:Houfeng(侯锋)
 * Email:Houzhanfeng@gmail.com
 * 
 *********************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxEngine.Utils
{
    /// <summary>
    /// 将一个方法标识为可在客户端调用的方法
    /// </summary>
    [Serializable]
    public sealed class AjaxMethodAttribute : System.Attribute
    {
        /// <summary>
        /// 构造
        /// </summary>
        public AjaxMethodAttribute()
        {
        }
    }
}
