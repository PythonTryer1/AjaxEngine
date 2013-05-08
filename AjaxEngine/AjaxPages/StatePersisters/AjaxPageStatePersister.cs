using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.Caching;
using System.Web;
using System.IO;
using System.Web.SessionState;

namespace AjaxEngine.AjaxPages.StatePersisters
{
    /// <summary>
    /// 提供操作视图状态基本功能
    /// </summary>
    /// 
    [Serializable]
    public class AjaxPageStatePersister : PageStatePersister
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="page"></param>
        public AjaxPageStatePersister(Page page) : base(page) { }
        /// <summary>
        /// 取得一个页面唯一值,此值将保持状态，不会因为回发而改变值。
        /// </summary>
        /// <returns>页面唯一Key</returns>
        public string GetUniqueKey()
        {
            string UniqueKey = Page.Request["__UNIQUEKEY"];
            if (UniqueKey== null || UniqueKey.Trim()== "")
            {
                UniqueKey = Page.Session.SessionID + DateTime.Now.Ticks;
            }
            Page.ClientScript.RegisterHiddenField("__UNIQUEKEY", UniqueKey);
            return UniqueKey;
        }
        /// <summary>
        /// 加载状态
        /// </summary>
        public override void Load()
        {
        }
        /// <summary>
        /// 保存状态
        /// </summary>
        public override void Save()
        {
        }

    }
}
