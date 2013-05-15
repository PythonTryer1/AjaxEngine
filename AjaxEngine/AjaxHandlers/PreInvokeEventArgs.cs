using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AjaxEngine.AjaxHandlers
{
    public class PreInvokeEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
        public object Result { get; set; }
    }
}
