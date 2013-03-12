using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.Caching;
using System.Web;
using System.IO;


namespace AjaxEngine.AjaxPages.StatePersisters
{
    /// <summary>
    /// 使用服务器缓存的视图状态功能类
    /// </summary>
    /// 
    [Serializable]
    public class CachePageStatePersister : AjaxPageStatePersister
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="page"></param>
        public CachePageStatePersister(Page page) : base(page) { }
        /// <summary>
        /// 加载视图状态
        /// </summary>
        public override void Load()
        {
            if (TheCache.Count >0 && TheCache["__Houfeng_AjaxEngine_" + this.GetUniqueKey()] !=null )
            {
                Pair pair = (Pair)this.Page.Cache["__Houfeng_AjaxEngine_" + this.GetUniqueKey()];
                base.ViewState = pair.First;
                base.ControlState = pair.Second;
            }
        }
        /// <summary>
        /// 保存视图状态
        /// </summary>
        public override void Save()
        {
            Pair pair = new Pair();
            if (base.ViewState != null)
                pair.First = base.ViewState;
            if (base.ControlState != null)
                pair.Second = base.ControlState;
            //
            //TheCache.Insert("__Houfeng_AjaxEngine_" + this.GetUniqueKey(), pair);
            TheCache.Insert("__Houfeng_AjaxEngine_" + this.GetUniqueKey(), pair, null,
            DateTime.Now.AddMinutes(this.Timeout), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);

        }
        /// <summary>
        /// 缓存超时时间（更改此值，请继承此类并重写此属性，并重写页面PageStatePersister属性返回自定义类实例）
        /// </summary>
        public virtual int Timeout
        {
            get
            {
                int timeout = 20;
                try
                {
                    if (Page.Session.Timeout > 0)
                        timeout = Page.Session.Timeout;
                }
                catch
                {}
                return timeout;
            }
        }
        /// <summary>
        /// 取缓存对象
        /// </summary>
        private Cache TheCache
        {
            get
            {
                return Page.Cache;
            }
        }
    }
}
