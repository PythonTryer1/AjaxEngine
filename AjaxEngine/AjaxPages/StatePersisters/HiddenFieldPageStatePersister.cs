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
    public class HiddenFieldPageStatePersister : System.Web.UI.HiddenFieldPageStatePersister
    {
        public HiddenFieldPageStatePersister(Page page) : base(page) { }
        public override void Load()
        {
            string viewStateString = Page.Request["__AJAXVIEWSTATE"];
            viewStateString = viewStateString.Replace(",", "").Replace(" ", "");
            LosFormatter los = new LosFormatter();
            Pair pair = (Pair)los.Deserialize(viewStateString);
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
            //
            LosFormatter los = new LosFormatter();
            StringWriter writer = new StringWriter();
            los.Serialize(writer, pair);
            //
            string viewStateString = writer.ToString();
            Page.ClientScript.RegisterHiddenField("__AJAXVIEWSTATE", viewStateString);
            if (((IAjaxPage)Page).PageEngine.IsAjaxPostBack)
            {
                ((IAjaxPage)Page).PageEngine.OutMessages.Add(new Message(MessageType.Script, "__AJAXVIEWSTATE", string.Format("document.getElementById('__AJAXVIEWSTATE').value='{0}'", viewStateString)));
            }
        }
    }
}
