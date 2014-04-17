using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AjaxEngine.AjaxHandlers;

namespace AjaxEngine.Test
{
    class Program
    {
        public class User
        {
            public string account { get; set; }
            public string password { get; set; }
            public User(string _account, string _password)
            {
                this.account = _account;
                this.password = _password;
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine(AjaxEngine.Gloabl.Serializer.Serialize(DateTime.Now));
            Console.Read();
            /*
            Client client = new Client();
            var data = new
            {
                method = "login",
                loginInfo = "{'account':'gptest\\\\guoqiang','password':'1'}",
                appKey = "aa"
            };
            var rs = client.Post<DataMap>("https://mo.chinastock.com.cn/service.ashx", data);
            Console.WriteLine(Gloabl.Serializer.Serialize(rs));
            Console.Read();
            */
        }
    }
}
