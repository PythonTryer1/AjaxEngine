using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AjaxEngine.Extends;
using System.Reflection;
using AjaxEngine.AjaxHandlers;

namespace AjaxEngine.Utils
{
    internal class ServiceDocBuilder
    {
        private static Dictionary<Type, string> DocCache = new Dictionary<Type, string>();
        public object Entity { get; set; }
        public ServiceDocBuilder(object entity)
        {
            this.Entity = entity;
        }
        public override string ToString()
        {
            Type entityType = this.Entity.GetType();
            if (DocCache.ContainsKey(entityType))
            {
                return DocCache[entityType];
            }
            string entityDescription = "";
            string entityName = entityType.Name;
            SummaryAttribute entitySummary = entityType.GetAttribute<SummaryAttribute>();
            if (entitySummary != null)
            {
                entityDescription = entitySummary.Description;
                if (!string.IsNullOrEmpty(entitySummary.Name))
                    entityName = entitySummary.Name;
            }

            StringBuilder buffer = new StringBuilder();
            buffer.Append(@"<!doctype html >
<html>
<head>
    <title>{entity}</title>
    <style>
        body { margin: 0px; color:#222; font-family: 微软雅黑, Verdana; font-size: 13px; }
        #header, #footer { padding: 8px; background-color: #333; color: #fdfdfd; }
        #header a, #footer a { color: #fdfdfd; }
        #header { font-size: 22px; font-weight: bold; }
        #content { padding: 8px; }
        .methodForm { border: solid 1px #bbb; padding: 8px; margin-bottom:10px; }
        .methodName { font-size:15px; padding-bottom: 8px; margin-bottom: 8px; display: block; border-bottom: dotted 1px #aaa; }
        table, td { border: 1px solid #ccc; border-collapse: collapse; }
        table{width:90%;}
        th, td { padding: 5px; }
        th { background-color: #e5e5ef; }
        .textbox { width:200px; }
        .button { margin-top: 5px; }
        #serviceSumary{margin-bottom:10px; padding:10px; border:solid 1px #bbb; }
    </style>
</head>
<body>
    <div id='header'>
        {entity}
    </div>
    <div id='content'><div id='serviceSumary'>说明 : {entitySumary}</div>".Replace("{entity}", entityName).Replace("{entitySumary}", entityDescription));

            List<MethodInfo> methodList = this.Entity.GetMethods().ToList().Where(m => m.GetAttribute<AjaxMethodAttribute>() != null).ToList();
            foreach (MethodInfo method in methodList)
            {
                string methodDescription = "";
                string methodReturn = "";
                Dictionary<string, string> methodParameters = new Dictionary<string, string>();
                SummaryAttribute methodSummary = method.GetAttribute<SummaryAttribute>();
                AjaxMethodAttribute ajaxMethod = method.GetAttribute<AjaxMethodAttribute>();
                if (methodSummary != null)
                {
                    methodDescription = methodSummary.Description ?? "";
                    methodReturn = methodSummary.Return ?? "";
                    string[] parameterDescArray = (methodSummary.Parameters ?? "").Split(',');
                    foreach (string parameterDesc in parameterDescArray)
                    {
                        string[] desc = parameterDesc.Split(':');
                        if (desc.Length > 1)
                            methodParameters.Add(desc[0], desc[1]);
                    }
                }
                buffer.Append(@"<form class='methodForm' target='_blank' method='{httpMethod}'>
<strong class='methodName'>{method}方法</strong>
说明 : {methodSumary}<br/>
返回 : {return}<br/>
<input name='method' type='hidden' value='{method}' />
参数 : <br/>
<table border='1'>
    <tr>
        <th style='width:125px;'>
            名称
        </th>
        <th style='width:205px;'>
            值
        </th>
        <th style='width:auto;'>
            类型
        </th>
        <th style='width:255px;'>
            说明
        </th>
    </tr>".Replace("{method}", method.Name).Replace("{return}", method.ReturnType.FullName + "," + methodReturn).Replace("{methodSumary}", methodDescription)).Replace("{httpMethod}", ajaxMethod.HttpMethod.Split(',')[0]);
                ParameterInfo[] parameterList = method.GetParameters();
                foreach (ParameterInfo parameter in parameterList)
                {
                    string pDesc = "";
                    methodParameters.TryGetValue(parameter.Name, out pDesc);
                    buffer.Append("<tr><td>{parameter}</td><td><input class='textbox' type='text' name='{parameter}'/></td><td>{parameter-type}</td><td>{parameter-desc}</td></tr>".Replace("{parameter}", parameter.Name).Replace("{parameter-type}", parameter.ParameterType.FullName)).Replace("{parameter-desc}", pDesc ?? "");
                }
                buffer.Append(@"</table>
        <input class='button' type='submit' value='调用' />
        </form>");
            }

            buffer.Append(@"</div>
<div id='footer'>
AjaxEngine {version}   -   Powered By <a href='http://www.houfeng.net' target='_blank'>Houfeng</a>
</div>".Replace("{version}", Assembly.GetExecutingAssembly().GetName().Version.ToString()));
            DocCache[entityType] = buffer.ToString();
            return DocCache[entityType];
        }
    }
}
