using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using AjaxEngine.Reflection;
using AjaxEngine.Serializes;
using AjaxEngine.Extends;

namespace AjaxEngine.Utils
{
    public class PageAgentBuilder
    {
        public static Dictionary<string, string> _Cache = new Dictionary<string, string>();
        public static Dictionary<string, string> Cache
        {
            get
            {
                return _Cache;
            }
        }
        public object Entity { get; set; }
        public string Namespace { get; set; }
        public PageAgentBuilder(string space, object entity)
        {
            this.Namespace = space;
            this.Entity = entity;
        }
        public override string ToString()
        {
            Type entityType = this.Entity.GetType();
            var cahceKey = entityType.FullName;
            if (!Cache.Keys.Contains(cahceKey))
            {
                var ns = this.Namespace;
                if (string.IsNullOrEmpty(ns))
                    ns = "window";
                else
                    ns = "window." + this.Namespace;
                //
                StringBuilder agentScriptBuffer = new StringBuilder(string.Format("{0}={0}||{{}};\r\n", ns));
                agentScriptBuffer.AppendLine("(function(owner){");
                MethodInfo[] methodList = entityType.GetMethods();
                foreach (MethodInfo method in methodList)
                {
                    AjaxMethodAttribute ajaxMethod = method.GetAttribute<AjaxMethodAttribute>();
                    if (ajaxMethod != null)
                        agentScriptBuffer.AppendLine("\t" + this.GenerateMethodAgentScript(method));
                }
                agentScriptBuffer.AppendLine("}(" + ns + "));");
                Cache.Add(cahceKey, agentScriptBuffer.ToString());
            }
            return Cache[cahceKey];
        }
        public string GenerateMethodAgentScript(MethodInfo method)
        {
            string buffer = "owner.{methodName}=function({parameters}){return AjaxEngine.callServer({ajaxParameters},_callback);}";
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
