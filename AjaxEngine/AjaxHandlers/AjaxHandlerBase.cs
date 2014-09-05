using AjaxEngine.Extends;
using AjaxEngine.Reflection;
using AjaxEngine.Serializes;
using AjaxEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace AjaxEngine.AjaxHandlers
{
    public class AjaxHandlerBase : IAjaxHandler
    {
        protected string HandleError(string message)
        {
            this.Context.Response.Clear();
            this.Context.Response.Write(message);
            this.Context.Response.End();
            return message;
        }
        protected string InvokeMethodName { get; set; }
        protected string JsonpCallbackName { get; set; }
        protected string JsonpEnabled { get; set; }
        protected IServiceEntity ServiceEntity { get; set; }
        protected ISerializer Serializer
        {
            get
            {
                return Gloabl.Serializer;
            }
        }
        protected HttpContext Context { get; set; }
        protected string AgentScriptNamespace { get; set; }
        public virtual bool IsReusable
        {
            get
            {
                return true;
            }
        }
        public virtual void ProcessRequest(HttpContext context)
        {
            this.Context = context;
            this.ServiceEntity = this;
            this.InvokeMethodName = Context.Request[Const.METHOD] ?? Context.Request.Headers[Const.METHOD];
            this.JsonpCallbackName = Context.Request[Const.CALLBACK] ?? Context.Request.Headers[Const.CALLBACK];
            //调用前
            HandleEventArgs processEventArgs = new HandleEventArgs();
            this.OnPrecess(processEventArgs);
            if (processEventArgs.Cancel)
            {
                context.Response.Clear();
                if (string.IsNullOrEmpty(this.JsonpCallbackName))
                {
                    context.Response.ContentType = Const.APPLICATION_JSON;
                    context.Response.Write(this.Serializer.Serialize(processEventArgs.Result));
                }
                else
                {
                    context.Response.ContentType = Const.APPLICATION_JAVASCRIPT;
                    context.Response.Write(string.Format("{0}({1});", this.JsonpCallbackName, this.Serializer.Serialize(processEventArgs.Result)));
                }
                context.Response.End();
            }
            //生成文档
            if (string.IsNullOrEmpty(context.Request[Const.METHOD])
                && string.IsNullOrEmpty(context.Request[Const.CLIENT_SCRIPT]))
            {
                ServiceDocBuilder docBuilder = new ServiceDocBuilder(this.ServiceEntity);
                context.Response.Clear();
                context.Response.ContentType = Const.TEXT_HTML;
                context.Response.Write(docBuilder.ToString());
                context.Response.End();
                return;
            }
            //生成代理脚本
            if (!string.IsNullOrEmpty(context.Request[Const.CLIENT_SCRIPT]))
            {
                var clientScript = context.Request[Const.CLIENT_SCRIPT].ToLower().Split(',');
                context.Response.Clear();
                context.Response.ContentType = Const.APPLICATION_JAVASCRIPT;
                if (clientScript.Contains(Const.JQUERY))
                {
                    context.Response.Write(Resources.Jquery);
                }
                if (clientScript.Contains(Const.CORE))
                {
                    context.Response.Write(Resources.ServiceCore);
                }
                if (clientScript.Contains(Const.AGENT))
                {
                    string ns = this.AgentScriptNamespace;
                    if (string.IsNullOrEmpty(ns))
                        ns = this.GetType().Name;
                    ServiceAgentBuilder agentBuilder = new ServiceAgentBuilder(context.Request.Url.ToString().Split('?')[0], ns, this.ServiceEntity);
                    context.Response.Write(agentBuilder.ToString());
                }
                context.Response.End();
                return;
            }
            //调用前
            HandleEventArgs invokeEventArgs = new HandleEventArgs();
            this.OnInvoke(invokeEventArgs);
            if (invokeEventArgs.Cancel)
            {
                context.Response.Clear();
                context.Response.Write(this.Serializer.Serialize(invokeEventArgs.Result));
                context.Response.End();
            }
            //调用
            if (!string.IsNullOrEmpty(this.InvokeMethodName))
            {
                object result = this.InvokeEntityMethod(this.InvokeMethodName, context.Request.HttpMethod);
                context.Response.Clear();
                if (string.IsNullOrEmpty(this.JsonpCallbackName))
                {
                    context.Response.ContentType = Const.APPLICATION_JSON;
                    context.Response.Write(this.Serializer.Serialize(result));
                }
                else
                {
                    context.Response.ContentType = Const.APPLICATION_JAVASCRIPT;
                    context.Response.Write(string.Format("{0}({1});", this.JsonpCallbackName, this.Serializer.Serialize(result)));
                }
                context.Response.End();
            }
            else
            {
                HandleError("没有找到指定调用方法");
            }
        }
        protected virtual object InvokeEntityMethod(string methodName, string httpMethod)
        {
            MethodInfo methodInfo = MethodFactory.GetMethodInfo(this.ServiceEntity.GetType(), methodName);
            string allowHttpMethods = null;
            if (methodInfo != null && this.IsAjaxMethod(methodInfo, ref allowHttpMethods))
            {
                if (!string.IsNullOrEmpty(allowHttpMethods) && allowHttpMethods.ToUpper().Split(',').Contains(httpMethod.ToUpper()))
                {
                    ParameterInfo[] pareameterInfos = ParameterFactory.GetPropertyInfo(methodInfo);
                    object[] parameterValueList = this.GetEntityParameterValueList(pareameterInfos);
                    return methodInfo.Invoke(this.ServiceEntity, parameterValueList);
                }
                else
                {
                    return HandleError("不允许用\"" + httpMethod + "\"方式调用");
                }
            }
            else
            {
                return HandleError("没有找到指定调用方法");
            }
        }
        protected virtual object[] GetEntityParameterValueList(ParameterInfo[] pareameterInfos)
        {
            List<object> valueList = new List<object>();
            foreach (ParameterInfo pi in pareameterInfos)
            {
                string valueString = Convert.ToString(this.Context.Request[pi.Name]);
                object value = valueString.ConvertTo(pi.ParameterType, null);
                if (value == null)
                {
                    value = Gloabl.Serializer.Deserialize(valueString, pi.ParameterType);
                }
                valueList.Add(value);
            }
            if (valueList.Count > 0)
                return valueList.ToArray();
            else
                return null;
        }
        protected virtual bool IsAjaxMethod(MethodInfo methodInfo, ref string httpMethod)
        {
            AjaxMethodAttribute ajaxMethod = methodInfo.GetAttribute<AjaxMethodAttribute>();
            httpMethod = ajaxMethod.HttpMethod;
            return ajaxMethod != null;
        }
        protected virtual void OnInvoke(HandleEventArgs invokeEventArgs)
        {
        }
        protected virtual void OnPrecess(HandleEventArgs requestEventArgs) { }
    }
}