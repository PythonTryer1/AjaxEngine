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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AjaxEngine.Extends;
using AjaxEngine.Reflection;
using AjaxEngine.Serializes;
using AjaxEngine.Utils;


namespace AjaxEngine.AjaxPages
{
    /// <summary>
    /// PageEngine引擎类
    /// </summary>
    [Serializable]
    public class AjaxPageEngine : IRequiresSessionState
    {
        #region 属性
        public string AjaxMethodNamespace { get; set; }
        public bool AutoImportJQuery { get; set; }
        public string InvokeMethodName { get; set; }
        public bool Enabled { get; set; }
        public Page Page { get; set; }
        /// <summary>
        /// 表单Html元素集合
        /// </summary>
        public HtmlElementCollection Html { get; set; }
        public bool IsAjaxRequest
        {
            get
            {
                return this.Page.Request["ajax-request"] != null;
            }
        }
        public bool IsAjaxPostBack
        {
            get
            {
                return IsAjaxRequest && this.Page.IsPostBack;
            }
        }
        public List<Message> OutMessages { get; set; }
        /// <summary>
        /// 是否为ASP.NET控件启用AJAX回发
        /// </summary>
        public bool ControlAjaxEnabled { get; set; }
        public ISerializer JsonSerializer
        {
            get
            {
                return Gloabl.Serializer;
            }
        }
        /// <summary>
        /// 是否自动更新控制呈现
        /// </summary>
        public bool AutoUpdateControls { get; set; }
        #endregion

        #region 构造及初始化方法
        /// <summary>
        /// 构造
        /// </summary>
        public AjaxPageEngine(Page page)
        {
            this.Page = page;
            this.InitAjaxEngine();
        }

        /// <summary>
        /// 初始
        /// </summary>
        private void InitAjaxEngine()
        {
            this.Enabled = true;
            this.OutMessages = new List<Message>();
            this.Html = new HtmlElementCollection(this);
            this.Page.PreRender += new EventHandler(Page_PreRender);
            this.ControlAjaxEnabled = true;
            this.AjaxMethodNamespace = "Server";
            this.AutoImportJQuery = true;
        }
        #endregion

        #region 核心功能相关方法
        private object InvokeEntityMethod(string methodName)
        {
            MethodInfo methodInfo = MethodCache.GetMethodInfo(this.Page.GetType(), methodName);
            if (methodInfo != null && this.IsAjaxMethod(methodInfo))
            {
                ParameterInfo[] pareameterInfos = ParameterCache.GetPropertyInfo(methodInfo);
                object[] parameterValueList = this.GetEntityParameterValueList(pareameterInfos);
                return methodInfo.Invoke(this.Page, parameterValueList);
            }
            else
            {
                throw new Exception("没有找到指定调用方法");
            }
        }
        private bool IsAjaxMethod(MethodInfo methodInfo)
        {
            return methodInfo.GetAttribute<AjaxMethodAttribute>() != null;
        }
        private object[] GetEntityParameterValueList(ParameterInfo[] pareameterInfos)
        {
            List<object> valueList = new List<object>();
            foreach (ParameterInfo pi in pareameterInfos)
            {
                string valueString = Convert.ToString(this.Page.Request[pi.Name]);
                object value = valueString.ConvertTo(pi.ParameterType);
                if (value == null)
                {
                    JsonSerializer serializer = new JsonSerializer();
                    value = serializer.Deserialize(valueString, pi.ParameterType);
                }
                valueList.Add(value);
            }
            if (valueList.Count > 0)
                return valueList.ToArray();
            else
                return null;
        }
        /// <summary>
        /// 处理所有AJAX请求
        /// </summary>
        /// <returns></returns>
        public AjaxPageEngine ProcessAjaxRequest()
        {
            if (!this.Enabled) return this;
            //
            if (!this.IsAjaxRequest)
                this.GenerateStripts();
            //
            this.InvokeMethodName = this.Page.Request[Const.METHOD];
            if (this.IsAjaxRequest && !string.IsNullOrEmpty(this.InvokeMethodName))
            {
                object result = this.InvokeEntityMethod(this.InvokeMethodName);
                this.OutMessages.Add(new Message(MessageType.Return, "return", result));
            }
            return this;
        }
        /// <summary>
        /// 生成脚本资源
        /// </summary>
        private void GenerateStripts()
        {
            if (!string.IsNullOrEmpty(Page.Request.QueryString[Const.CLIENT_SCRIPT]))
            {
                var clientScript = Page.Request.QueryString[Const.CLIENT_SCRIPT].ToLower().Split(',');
                Page.Response.Clear();
                Page.Response.ContentType = Const.TEXT_JAVASCRIPT;
                if (clientScript.Contains(Const.JQUERY))
                {
                    Page.Response.Write(Resources.Jquery);
                }
                if (clientScript.Contains(Const.CORE))
                {
                    Page.Response.Write(Resources.PageCore);
                }
                if (clientScript.Contains(Const.AGENT))
                {
                    PageAgentBuilder builder = new PageAgentBuilder(this.AjaxMethodNamespace, this.Page);
                    Page.Response.Write(builder.ToString());
                }
                Page.Response.End();
            }
            else
            {
                this.RegisterClientScriptBlock(string.Format("<script type='text/javascript'>__ControlAjaxEnabled={0};</script>", this.ControlAjaxEnabled.ToString().ToLower()));
                // this.RegisterClientScriptInclude(Page.ClientScript.GetWebResourceUrl(this.GetType(), "AjaxEngine.Scripts.page-core.js"));
                if (this.AutoImportJQuery)
                    this.RegisterClientScriptInclude(string.Format("{0}?{1}={2},{3},{4}", this.GetPageFileName(), Const.CLIENT_SCRIPT, Const.JQUERY, Const.CORE, Const.AGENT));
                else
                    this.RegisterClientScriptInclude(string.Format("{0}?{1}={2},{3}", this.GetPageFileName(), Const.CLIENT_SCRIPT, Const.CORE, Const.AGENT));
            }
        }
        /// <summary>
        /// 以AJAX方法呈现页面
        /// </summary>
        public void AjaxRender()
        {
            if (!this.Enabled) return;
            if (this.IsAjaxRequest)
            {
                if (this.AutoUpdateControls)
                    this.RenderAllControls();
                //
                Page.Response.Clear();
                Page.Response.Write(this.JsonSerializer.Serialize(this.OutMessages));
                Page.Response.End();
            }
        }
        #endregion

        #region 页面处理方法
        /// <summary>
        /// 页面预呈现事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Page_PreRender(object sender, EventArgs e)
        {
            Page.ClientScript.GetPostBackEventReference(Page, "");
            this.ProcessButton(Page.Form);
        }
        /// <summary>
        /// 将一个控件呈现到String.
        /// </summary>
        /// <param name="control">控件</param>
        /// <returns>控件呈现字符串</returns>
        public string ControlRenderToString(Control control)
        {
            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
            if (ControlReader != null)
                ControlReader(this, new ControlRenderEvenArgs(control));
            control.RenderControl(htmlWriter);
            StringBuilder stringBuilder = stringWriter.GetStringBuilder();
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 控件呈现事件在呈现一个控件时触发
        /// </summary>
        public event ControlRenderEventHandler ControlReader;
        /// <summary>
        /// 初始化并将所有控制更新消息添加到消息列表
        /// </summary>
        public void RenderAllControls()
        {
            List<Message> renderMsgList = this.OutMessages.Where(msg => msg.Type == MessageType.Render).ToList();
            foreach (Message msg in renderMsgList)
                this.OutMessages.Remove(msg);
            foreach (Control control in Page.Form.Controls)
                this.OutMessages.Insert(0, new Message(MessageType.Render, control.ClientID, this.ControlRenderToString(control)));
        }
        #endregion

        #region 引擎功能相关方法
        /// <summary>
        /// 更新一个控件
        /// </summary>
        /// <param name="control"></param>
        public void UpdateControlRender(Control control)
        {
            this.OutMessages.Insert(0, new Message(MessageType.Render, control.ClientID, this.ControlRenderToString(control)));
        }
        /// <summary>
        /// 取消更新一个控件
        /// </summary>
        /// <param name="control"></param>
        public void CancelUpdateControlRender(Control ct)
        {
            foreach (Message msg in this.OutMessages)
            {
                if (msg.Id == ct.ID)
                    this.OutMessages.Remove(msg);
            }
        }
        /// <summary>
        /// 移除一个控件Ajax能力 （ControlsAjaxEnabled为true时有效）
        /// </summary>
        /// <param name="control">控件</param>
        public void RemoveAjaxAble(Control control)
        {
            try
            {
                ((WebControl)control).Attributes.Add("ajax-disabled", "yes");
            }
            catch { }
            try
            {
                ((HtmlControl)control).Attributes.Add("ajax-disabled", "yes");
            }
            catch { }
        }
        /// 添加一个控件的Ajax能力 （ControlsAjaxEnabled为true时有效）
        /// </summary>
        /// <param name="control">控件</param>
        public void AddAjaxAble(Control control)
        {
            try
            {
                ((WebControl)control).Attributes.Remove("ajax-disabled");
            }
            catch { }
            try
            {
                ((HtmlControl)control).Attributes.Remove("ajax-disabled");
            }
            catch { }
        }
        /// <summary>
        /// 禁用按扭的自运回发属性
        /// </summary>
        /// <param name="control">ctrl控件（其子控件中的所有Button都会受影响）</param>
        public void ProcessButton(Control control)
        {
            try
            {
                Button btn = (Button)control;
                if (btn.OnClientClick.IndexOf("__doPostBack(") < 0)
                    btn.OnClientClick += @"__doPostBack('" + btn.UniqueID + "', '');return false;";
            }
            catch { }
            //
            ControlCollection controls = control.Controls;
            if (controls.Count > 0)
            {
                foreach (Control ct in controls)
                {
                    this.ProcessButton(ct);
                }
            }
        }

        #endregion

        #region 其它功能相关方法

        /// <summary>
        /// 取得页面文件名
        /// </summary>
        /// <returns>页文件名</returns>
        public string GetPageFileName()
        {
            //
            string url = Page.Request.FilePath.ToString();
            string file = url.Substring(url.LastIndexOf("/") + 1);
            return file;
        }
        /// <summary>
        /// 生成一个页面唯一字串
        /// </summary>
        /// <returns>页唯一字符</returns>
        public string GetPageTempGuid()
        {
            //
            string url = Page.Request.FilePath.ToString();
            string ip = Page.Request.UserHostAddress;
            string name = Page.Request.UserHostName;
            return name + ip + url;
        }
        #endregion

        #region 页面脚本相关方法
        /// <summary>
        /// 注册脚本
        /// </summary>
        /// <param name="script">脚本</param>
        public void RegisterStartupScript(string script)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), Guid.NewGuid().ToString(), script);
        }
        public void RegisterClientScriptInclude(string url)
        {
            Page.ClientScript.RegisterClientScriptInclude(Page.GetType(), Guid.NewGuid().ToString(), url);
        }
        public void RegisterClientScriptBlock(string script)
        {
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), Guid.NewGuid().ToString(), script);
        }
        /// <summary>
        /// 执行一段客户端脚本
        /// </summary>
        /// <param name="script">客户端脚本</param>
        public void InvokeClientScript(string script)
        {
            if (IsAjaxPostBack)
                this.OutMessages.Add(new Message(MessageType.Script, "script", script));
            else
                this.RegisterStartupScript("<script>" + script + "</script>");
        }
        /// <summary>
        /// 在客户端显示一个消息框
        /// </summary>
        /// <param name="text">消息文本</param>
        public void ShowMessageBox(string text, string okScript)
        {
            this.InvokeClientScript(string.Format("alert(\"{0}\");{1}", TextHelper.FilterString(text), okScript));
        }
        public void ShowMessageBox(string text)
        {
            this.ShowMessageBox(text, "");
        }
        public void ShowMessageBox<T>(string text, CallbackHandler<T> callback, T arg)
        {
            if (!this.IsAjaxMethod(callback.Method))
                throw new Exception(string.Format("没有找到方法:{0}", callback.Method.Name));
            string argString = this.JsonSerializer.Serialize(arg);
            string call = string.Format("{0}.{1}({2});", this.AjaxMethodNamespace, callback.Method.Name, argString);
            this.ShowMessageBox(text, call);
        }
        /// <summary>
        /// 在客户端显示一个有“是”和“否”按钮的确认框
        /// </summary>
        /// <param name="text">提示文本</param>
        /// <param name="okScript">按下“是”执行的脚本</param>
        /// <param name="cancelScript">按下否执行的脚本</param>
        public void ShowConfirmBox(string text, string okScript, string cancelScript)
        {
            this.InvokeClientScript("if(confirm(\"" + TextHelper.FilterString(text) + "\")){" + okScript + "}else{" + cancelScript + "}");
        }
        public void ShowConfirmBox<T>(string text, CallbackHandler<T> okCallback, CallbackHandler<T> cancelCallback, T arg)
        {
            if (!this.IsAjaxMethod(okCallback.Method))
                throw new Exception(string.Format("没有找到方法:{0}", okCallback.Method.Name));
            if (!this.IsAjaxMethod(cancelCallback.Method))
                throw new Exception(string.Format("没有找到方法:{0}", cancelCallback.Method.Name));
            string argString = this.JsonSerializer.Serialize(arg);
            string okCall = string.Format("{0}.{1}({2});", this.AjaxMethodNamespace, okCallback.Method.Name, argString);
            string cancelCall = string.Format("{0}.{1}({2});", this.AjaxMethodNamespace, cancelCallback.Method.Name, argString);
            this.ShowConfirmBox(text, okCall, cancelCall);
        }
        /// <summary>
        /// 向指定的控件中输出一段字符串(并不能保持状态)
        /// </summary>
        /// <param name="str">文本</param>
        /// <param name="ControlClinetID">控件客户端ID或Html标签ID</param>
        public void WriteString(string str, string ControlClinetID)
        {
            this.InvokeClientScript("document.getElementById('" + ControlClinetID + "').innerHTML+=\"" + TextHelper.FilterString(str) + "\";");
        }
        /// <summary>
        /// 输出字符串
        /// </summary>
        /// <param name="txt"></param>
        public void WriteString(string txt)
        {
            this.WriteString(txt, this.Page.Form.ClientID);
        }
        /// <summary>
        /// 转到指定的url
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="target">目标窗口</param>
        public void OpenWindow(string url, string target, string option)
        {
            this.InvokeClientScript(string.Format("var __win=window.open(\"{0}\",\"{1}\",\"{2}\");__win.focus();", TextHelper.FilterString(url), TextHelper.FilterString(target), TextHelper.FilterString(option)));
        }
        /// <summary>
        /// 转到指定的url
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="target">目标窗口</param>
        public void OpenWindow<T1, T2>(string url, string target, string option, T1 arg, CallbackHandler<T2> callback)
        {
            if (url.Contains("?"))
                url += string.Format("&__window_callback={0}.{1}&__window_args={2}", this.AjaxMethodNamespace, callback.Method.Name, this.JsonSerializer.Serialize(arg));
            else
                url += string.Format("?__window_callback={0}.{1}&__window_args={2}", this.AjaxMethodNamespace, callback.Method.Name, this.JsonSerializer.Serialize(arg));
            this.OpenWindow(url, target, option);
        }
        public T GetWindowArgs<T>()
        {
            string value = this.Page.Request.QueryString["__window_args"];
            if (!string.IsNullOrEmpty(value))
            {
                return this.JsonSerializer.Deserialize<T>(value);
            }
            return default(T);
        }
        public void ReturnValue(object value)
        {
            string callback = this.Page.Request.QueryString["__window_callback"];
            this.InvokeClientScript(string.Format("window.opener&&window.opener.{0}({1});", callback, this.JsonSerializer.Serialize(value)));
        }
        public void CloseWindow()
        {
            this.InvokeClientScript("window.close();");
        }
        /// <summary>
        /// 转到指定的url
        /// </summary>
        /// <param name="url">url</param>
        public void GotoUrl(string url)
        {
            if (IsAjaxPostBack)
                this.InvokeClientScript("location.href=\"" + TextHelper.FilterString(url) + "\";");
            else
                Page.Response.Redirect(url);
        }
        #endregion

    }
}