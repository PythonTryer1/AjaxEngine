using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.SessionState;
using AjaxEngine.Extends;
using AjaxEngine.Reflection;
using AjaxEngine.Serializes;
using AjaxEngine.Utils;

namespace AjaxEngine.AjaxHandlers
{
    public class AjaxHandlerBase : IAjaxHandler
    {
        protected virtual Error HandleError(Exception ex)
        {
            if (this.ThrowError)
                throw ex;
            else
                return new Error(ex.Message, ex.Source, ex.TargetSite.Name);
        }
        protected virtual bool ThrowError { get; set; }
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
            this.ServiceEntity = this;
            this.Context = context;
            //
            var invoke = this.PreInvoke();
            //生成文档
            if (context.Request.QueryString.Count < 1 && context.Request.Form.Count < 1)
            {
                ServiceDocBuilder docBuilder = new ServiceDocBuilder(this.ServiceEntity);
                context.Response.Clear();
                context.Response.Write(docBuilder.ToString());
                context.Response.End();
                return;
            }
            //生成代理脚本
            if (!string.IsNullOrEmpty(context.Request.QueryString[Const.CLIENT_SCRIPT]))
            {
                var clientScript = context.Request.QueryString[Const.CLIENT_SCRIPT].ToLower().Split(',');
                context.Response.Clear();
                context.Response.ContentType = Const.TEXT_JAVASCRIPT;
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
            //处理调用
            this.InvokeMethodName = context.Request[Const.METHOD];
            this.JsonpCallbackName = context.Request[Const.CALLBACK];
            if (invoke && !string.IsNullOrEmpty(this.InvokeMethodName))
            {
                object result = this.InvokeEntityMethod(this.InvokeMethodName, context.Request.HttpMethod);
                context.Response.Clear();
                if (string.IsNullOrEmpty(this.JsonpCallbackName))
                    context.Response.Write(this.Serializer.Serialize(result));
                else
                    context.Response.Write(string.Format("{0}({1});", this.JsonpCallbackName, this.Serializer.Serialize(result)));
                context.Response.End();
            }
            else
            {
                context.Response.Clear();
                context.Response.Write(this.Serializer.Serialize(HandleError(new Exception("没有找到指定调用方法"))));
                context.Response.End();
            }
        }
        protected virtual object InvokeEntityMethod(string methodName, string httpMethod)
        {

            MethodInfo methodInfo = MethodCache.GetMethodInfo(this.ServiceEntity.GetType(), methodName);
            string allowHttpMethods = null;
            if (methodInfo != null && this.IsAjaxMethod(methodInfo, ref allowHttpMethods))
            {
                if (!string.IsNullOrEmpty(allowHttpMethods) && allowHttpMethods.ToUpper().Split(',').Contains(httpMethod.ToUpper()))
                {
                    try
                    {
                        ParameterInfo[] pareameterInfos = ParameterCache.GetPropertyInfo(methodInfo);
                        object[] parameterValueList = this.GetEntityParameterValueList(pareameterInfos);
                        return methodInfo.Invoke(this.ServiceEntity, parameterValueList);
                    }
                    catch (Exception ex)
                    {
                        return HandleError(ex);
                    }
                }
                else
                {
                    return HandleError(new Exception("不允许用\"" + httpMethod + "\"方式调用"));
                }
            }
            else
            {
                return HandleError(new Exception("没有找到指定调用方法"));
            }
        }
        protected virtual object[] GetEntityParameterValueList(ParameterInfo[] pareameterInfos)
        {
            List<object> valueList = new List<object>();
            foreach (ParameterInfo pi in pareameterInfos)
            {
                string valueString = Convert.ToString(this.Context.Request[pi.Name]);
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
        protected virtual bool IsAjaxMethod(MethodInfo methodInfo, ref string httpMethod)
        {
            AjaxMethodAttribute ajaxMethod = methodInfo.GetAttribute<AjaxMethodAttribute>();
            httpMethod = ajaxMethod.HttpMethod;
            return ajaxMethod != null;
        }
        protected virtual bool PreInvoke()
        {
            return true;
        }
    }
}