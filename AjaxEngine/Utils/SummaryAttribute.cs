using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AjaxEngine.Utils
{
    /// <summary>
    /// 描述
    /// </summary>
    [Serializable]
    public sealed class SummaryAttribute : System.Attribute
    {
        public string Description { get; set; }
        public string Return { get; set; }
        public string Parameters { get; set; }
        /// <summary>
        /// 构造
        /// </summary>
        public SummaryAttribute()
        {
        }
    }
}
