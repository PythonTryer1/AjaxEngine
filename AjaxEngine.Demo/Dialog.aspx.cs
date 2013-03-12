using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxEngine.AjaxPages;

namespace AjaxEngine.Demo
{
    public partial class Dialog : AjaxPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            this.PageEngine.ReturnValue(this.PageEngine.GetWindowArgs<T>());
            this.PageEngine.CloseWindow();
        }
    }
}