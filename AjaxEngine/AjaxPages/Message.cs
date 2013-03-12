/**********************************************************
 * 
 * AjaxEngine(Houfeng.Web.Ajax) 
 * -------------------------------------------------------
 * 作者:Houfeng(侯锋)
 * Email:Houzhanfeng@gmail.com
 * 
 *********************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxEngine.AjaxPages
{
    [Serializable]
    public class Message
    {
        /// <summary>
        /// 构造一个消息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="context"></param>
        public Message(MessageType type, string id, object context)
        {
            this.Type = type;
            this.Id = id;
            this.Context = context;
        }
        /// <summary>
        /// 构造一个消息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="context"></param>
        public Message(MessageType type, object context)
        {
            this.Type = type;
            this.Id = type.ToString().ToLower();
            this.Context = context;
        }
        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType Type { get; set; }
        /// <summary>
        /// 消息ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public object Context { get; set; }
    }

}
