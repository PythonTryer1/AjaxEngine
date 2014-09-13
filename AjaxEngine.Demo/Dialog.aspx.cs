using AjaxEngine.AjaxPages;
using System;

namespace AjaxEngine.Demo
{
    public partial class Dialog : AjaxPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var url = "~/efg";
            this.PageEngine.WriteString(this.PageEngine.ResolveUrl(url));
            this.PageEngine.WriteString("<br/>");
            this.PageEngine.WriteString(this.ResolveUrl(url));
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            this.PageEngine.ReturnValue(this.PageEngine.GetWindowArgs<T>());
            this.PageEngine.CloseWindow();
        }
    }
}