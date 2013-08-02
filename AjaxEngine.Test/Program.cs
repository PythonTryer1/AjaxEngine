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
            Client client = new Client();
            var data = new { 
                method="login",
                user = "{'account':'gptest\\\\guoqiang','password':'1'}"//new User("gptest\\guoqiang", "1")
            };
            var rs = client.Post<DataMap>("http://192.168.1.107/service.ashx", data);
            Console.Write(Gloabl.Serializer.Serialize(rs));
            Console.Read();
        }
    }
}
