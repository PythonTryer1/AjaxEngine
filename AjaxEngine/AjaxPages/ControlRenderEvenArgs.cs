using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace AjaxEngine.AjaxPages
{
    public delegate void ControlRenderEventHandler(object sender, ControlRenderEvenArgs e);
    public class ControlRenderEvenArgs : EventArgs
    {
        public Control Control { get; set; }
        public ControlRenderEvenArgs(Control ct)
        {
            this.Control = ct;
        }
    }
}
