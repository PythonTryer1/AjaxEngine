using System;

namespace AjaxEngine.AjaxHandlers
{
    public class HandleEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
        public object Result { get; set; }
    }
}
