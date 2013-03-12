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
using System.Text;
using System.Web.Script.Serialization;

namespace AjaxEngine.Serializes
{
    public class JsonSerializer :ISerializer
    {
        private JavaScriptSerializer innerJsonSerializer = new JavaScriptSerializer();
        /// <summary>
        /// Json序列化器
        /// </summary>
        public JsonSerializer()
        {
            this.innerJsonSerializer = new JavaScriptSerializer();
            //JavaScriptConverter[] customConverter = new JavaScriptConverter[] { new DateTimeConverter() };
            //this.innerJsonSerializer.RegisterConverters(customConverter);
            this.innerJsonSerializer.MaxJsonLength = int.MaxValue;
        }
        public string Serialize(object obj) {
            return this.innerJsonSerializer.Serialize(obj);
        }
        public void Serialize(object obj,StringBuilder output) {
            this.innerJsonSerializer.Serialize(obj, output);
        }
        public T Deserialize<T>(string text) {
            return this.innerJsonSerializer.Deserialize<T>(text);
        }
        public object Deserialize(string text,Type type){
            return this.innerJsonSerializer.Deserialize(text,type);
        }
    }
}
