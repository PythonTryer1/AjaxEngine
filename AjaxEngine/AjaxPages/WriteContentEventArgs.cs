using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxEngine.AjaxPages
{
    [Serializable]
    public class WriteContentEventArgs : EventArgs
    {
        public WriteContentEventArgs(List<Message> outMessages, bool canel)
        {
            this.OutMessages = outMessages;
            this.Canel = canel;
        }
        /// <summary>
        /// 待输出的消息
        /// </summary>
        public List<Message> OutMessages { get; set; }
        /// <summary>
        /// 是否取消响应，如果为true则取消，将输出正常的页面。
        /// </summary>
        public bool Canel { get; set; }
    }

    public delegate void WriterContentEventHandler(Object sender, WriteContentEventArgs e);
}
