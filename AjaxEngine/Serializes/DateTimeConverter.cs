/*
 * 版本: 0.1
 * 描述: 对象序列化为json时，为时间类型提供转换功能。
 * 创建: Houfeng
 * 邮件: admin@xhou.net
 * 
 * 修改记录:
 * 2011-11-7,Houfeng,添加文件说明，更新版本号为0.1
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace AjaxEngine.Serializes
{
    public class DateTimeConverter : JavaScriptConverter
    {
        public override IEnumerable<Type> SupportedTypes
        {
            get { return new List<Type>() { typeof(DateTime), typeof(DateTime?) }; }
        }
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (obj == null) return result;
            result["value"] = ((DateTime)obj).Ticks;
            return result;
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            if (dictionary.ContainsKey("value"))
                return new DateTime((long)dictionary["value"], DateTimeKind.Unspecified);
            return null;
        }
    }
}
