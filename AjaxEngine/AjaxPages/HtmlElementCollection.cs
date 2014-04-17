using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using AjaxEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace AjaxEngine.AjaxPages
{ /// <summary>
    /// HMTL元素
    /// </summary>
    /// 
    [Serializable]
    public class HtmlElementCollection : List<HtmlElement>
    {
        Page _page;
        AjaxPageEngine _AjaxEngine;
        /// <summary>
        /// 构造一个表单元素集合
        /// </summary>
        /// <param name="page"></param>
        public HtmlElementCollection(AjaxPageEngine ajaxengine)
        {
            _AjaxEngine = ajaxengine;
            _page = ajaxengine.Page;
            Init();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            NameObjectCollectionBase.KeysCollection FormElements = _page.Request.Form.Keys;
            int count = FormElements.Count;
            for (int i = 0; i < count; i++)
            {
                this.Add(new HtmlElement(_AjaxEngine, FormElements[i]));
            }

        }
        /// <summary>
        /// 查找一个元素
        /// </summary>
        /// <param name="htmlName"></param>
        /// <returns></returns>
        private int FindHtmlElement(string htmlName)
        {
            foreach (HtmlElement e in this)
            {
                if (e.Name == htmlName)
                    return this.IndexOf(e);
            }
            return -1;
        }
        /// <summary>
        /// 取得指定name/id的html元素
        /// </summary>
        /// <param name="htmlid"></param>
        /// <returns></returns>
        public HtmlElement this[string htmlName]
        {
            get
            {
                int index = this.FindHtmlElement(htmlName);
                if (index == -1)
                {
                    this.Add(new HtmlElement(_AjaxEngine, htmlName));
                    index = this.Count - 1;
                }
                return this[index];
            }
            set
            {
                int index = this.FindHtmlElement(htmlName);
                if (index != -1)
                    this[index] = value;
                else
                    this.Add(new HtmlElement(_AjaxEngine, htmlName));
            }
        }
    }
}
