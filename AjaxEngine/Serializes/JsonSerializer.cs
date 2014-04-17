/*
 * 版本: 0.1
 * 描述: json序列化类。
 * 创建: Houfeng
 * 邮件: houzf@prolliance.cn
 * 
 * 修改记录:
 * 2011-11-7,Houfeng,添加文件说明，更新版本号为0.1
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Web.Script.Serialization;
using AjaxEngine.Json;
using AjaxEngine.Json.Converters;

namespace AjaxEngine.Serializes
{
    public class JsonSerializer : ISerializer
    {
        //private JavaScriptSerializer serializer = new JavaScriptSerializer();
        
        public string Serialize(object obj)
        {
            var dateTimeFormat = new IsoDateTimeConverter();
            dateTimeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss.ffff";
            //return serializer.Serialize(obj);
            return JsonConvert.SerializeObject(obj, Formatting.None, dateTimeFormat);
        }
        public T Deserialize<T>(string text)
        {
            //return serializer.Deserialize<T>(text);
            return JsonConvert.DeserializeObject<T>(text);
        }
        public object Deserialize(string text, Type type)
        {
            //JsonQueryStringConverter ser = new JsonQueryStringConverter();
            //return ser.ConvertStringToValue(text, type);
            return JsonConvert.DeserializeObject(text, type);
        }
    }
}
