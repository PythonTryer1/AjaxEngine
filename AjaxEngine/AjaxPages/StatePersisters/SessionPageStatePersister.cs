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
    [Serializable]
    public class SessionPageStatePersister : AjaxPageStatePersister 
    {
        public SessionPageStatePersister(Page page) : base(page) { }
        public override void Load()
        {
            Pair pair = (Pair)this.Page.Session["__Houfeng_AjaxEngine_" + this.GetUniqueKey()];
            base.ViewState = pair.First;
            base.ControlState = pair.Second;
        }
        public override void Save()
        {
            Pair pair = new Pair();
            if (base.ViewState != null)
            { pair.First = base.ViewState; }
            if (base.ControlState != null)
            { pair.Second = base.ControlState; }
            this.Page.Session["__Houfeng_AjaxEngine_"+this.GetUniqueKey()] =pair; 
        }
    }
}
