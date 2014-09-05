using AjaxEngine.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AjaxEngine.Utils
{
    public class ServiceAgentBuilder
    {
        private static Dictionary<string, string> _Cache = new Dictionary<string, string>();
        public static Dictionary<string, string> Cache
        {
            get
            {
                return _Cache;
            }
        }
        public object Entity { get; set; }
        public string Namespace { get; set; }
        public string Url { get; set; }
        public ServiceAgentBuilder(string url, string space, object entity)
        {
            this.Url = url;
            this.Namespace = space;
            this.Entity = entity;
        }
        public override string ToString()
        {
            Type entityType = this.Entity.GetType();
            var cacheKey = this.Url + entityType.FullName;
            if (!Cache.Keys.Contains(cacheKey))
            {
                var ns = this.Namespace;
                if (string.IsNullOrEmpty(ns))
                    ns = "window";
                else
                    ns = "window." + this.Namespace;
                //
                StringBuilder agentScriptBuffer = new StringBuilder(string.Format("{0}={0}||{{}};\r\n", ns));
                agentScriptBuffer.AppendLine("(function(owner){");
                agentScriptBuffer.AppendLine(string.Format("\towner.url='{0}';", this.Url));
                agentScriptBuffer.AppendLine("\towner.crossDomain=false;");
                agentScriptBuffer.AppendLine("\towner.getUrl=function(){if(owner.crossDomain){return owner.url+'?callback=?';}else{return owner.url;}};");
                MethodInfo[] methodList = entityType.GetMethods();
                foreach (MethodInfo method in methodList)
                {
                    AjaxMethodAttribute ajaxMethod = method.GetAttribute<AjaxMethodAttribute>();
                    if (ajaxMethod != null)
                        agentScriptBuffer.AppendLine("\t" + this.GenerateMethodAgentScript(method));
                }
                agentScriptBuffer.AppendLine("}(" + ns + "));\r\nif(typeof define ==='function' && define.amd){ define('" + this.Namespace + "',[],function(){return " + ns + "; }); }");
                Cache.Add(cacheKey, agentScriptBuffer.ToString());
            }
            return Cache[cacheKey];
        }
        public string GenerateMethodAgentScript(MethodInfo method)
        {
            string buffer = "owner.{methodName}=function({parameters}){return AjaxEngine.callService(owner.getUrl(),{ajaxParameters},_callback);}";
            StringBuilder parametersBuffer = new StringBuilder();
            StringBuilder ajaxParametersBuffer = new StringBuilder(string.Format("{{'method':'{0}'", method.Name));
            ParameterInfo[] parameters = method.GetParameters();
            foreach (ParameterInfo parameter in parameters)
            {
                string scriptParameterName = string.Format("_{0}", parameter.Name.ToLower());
                parametersBuffer.Append(string.Format("{0},", scriptParameterName));
                ajaxParametersBuffer.Append(string.Format(",'{0}':{1}", parameter.Name, scriptParameterName));
            }
            parametersBuffer.Append("_callback");
            ajaxParametersBuffer.Append("}");
            return buffer.Replace("{methodName}", method.Name).Replace("{parameters}", parametersBuffer.ToString()).Replace("{ajaxParameters}", ajaxParametersBuffer.ToString());
        }
    }
}
