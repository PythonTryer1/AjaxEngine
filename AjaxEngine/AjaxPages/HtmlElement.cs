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
using System.Collections.Generic ;
using System.Collections.Specialized;
using AjaxEngine.Utils;

namespace AjaxEngine.AjaxPages
{
   
    /// <summary>
    /// 一个HMTL元素
    /// </summary>
    /// 
    [Serializable]
    public class HtmlElement
    {
        Page _page;
        AjaxPageEngine _AjaxEngine;
        /// <summary>
        /// 实例一个HTML元素类
        /// </summary>
        /// <param name="page"></param>
        /// <param name="htmlid"></param>
        public HtmlElement(AjaxPageEngine ajaxengine, string htmlName)
        {
            _AjaxEngine = ajaxengine;
            _page = ajaxengine.Page;
            this.Name = htmlName;
        }
        /// <summary>
        /// 实例一个HTML元素类
        /// </summary>
        /// <param name="page"></param>
        public HtmlElement(AjaxPageBase page)
        {
            _page = page;
        }
        public void GetValueFormClient()
        {
             _Value = _page.Request[_HtmlName];
        }
        private string _HtmlName;
        /// <summary>
        /// 元素ID
        /// </summary>
        public string Name
        {
            get
            {
                return _HtmlName ;
            }
            set
            {
                _HtmlName = value;
                GetValueFormClient();
            }
        }
        private string _Value;
        /// <summary>
        /// 元素值
        /// </summary>
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                string str="document.getElementsByName('" + _HtmlName + "')[0].value=\"" + value + "\";";
               // _AjaxEngine.OutMessages.Add(new Message(MessageType.Script, str));
                _AjaxEngine.InvokeClientScript(str);
                _Value = value;
            }
        }
        public string ChcekedValue
        {
            get
            {
                return this.Value;
            }
            set
            {
                string[] vals = value.Split('-');
                foreach (string val in vals)
                {
                    string str = "function ________SetCheckValue(){var els=document.getElementsByName('" + _HtmlName + "');var elscount=els.length;for(var i=0;i<elscount;i++){if(els[i].value=='" + TextHelper.FilterString(val) + "'){els[i].checked='checked';}} }________SetCheckValue();";
                    // _AjaxEngine.OutMessages.Add(new Message(MessageType.Script, str));
                    _AjaxEngine.InvokeClientScript(str);
              }
                _Value = (value);
            }
        }
        /// <summary>
        /// 为元素添加特性
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddAttributes(string key, string value)
        {
            _AjaxEngine.InvokeClientScript("document.getElementsByName('" + _HtmlName + "')[0].attributes['" + key + "'].value=\"" + TextHelper.FilterString(value) + "\";");
        }
        /// <summary>
        /// 设置元元素innerText
        /// </summary>
        public string innerText
        {
            set
            {
                _AjaxEngine.InvokeClientScript("document.getElementsByName('" + _HtmlName + "')[0].innerText=\"" + TextHelper.FilterString(value) + "\";");
            }
        }
        /// <summary>
        /// 设置元素innerHTML
        /// </summary>
        public string innerHTML
        {
            set
            {
                _AjaxEngine.InvokeClientScript("document.getElementsByName('" + _HtmlName + "')[0].innerHTML=\"" + TextHelper.FilterString(value) + "\";");
            }
        }
        /// <summary>
        /// 将元素替换为指定HTML字符串
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
        public void ReplaceWith(string htmlStr)
        {
            _AjaxEngine.InvokeClientScript("document.getElementsByName('" + _HtmlName + "')[0].outerHTML=\"" + TextHelper.FilterString(htmlStr) + "\";");
        }
    }

}
