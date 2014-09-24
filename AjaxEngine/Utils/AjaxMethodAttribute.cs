/**********************************************************
 * 
 * AjaxAttribute 用于将方法标记为Ajax方法 
 * -------------------------------------------------------
 * 作者:Houfeng(侯锋)
 * Email:Houzhanfeng@gmail.com
 * 
 *********************************************************/

using System;

namespace AjaxEngine.Utils
{
    /// <summary>
    /// 将一个方法标识为可在客户端调用的方法
    /// </summary>
    [Serializable]
    public sealed class AjaxMethodAttribute : Attribute
    {
        private string _HttpMethod = "POST,GET";

        /// <summary>
        /// 允许的 Http 请求方法
        /// </summary>
        public string HttpMethod
        {
            get
            {
                return _HttpMethod ?? "";
            }
            set
            {
                _HttpMethod = (value ?? "");
            }
        }

        /// <summary>
        /// 别名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 构造
        /// </summary>
        public AjaxMethodAttribute()
        {

        }
    }
}
