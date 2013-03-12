using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AjaxEngine.Utils
{
    public class Error
    {
        public string message { get; set; }
        public string source { get; set; }
        public string target { get; set; }
        public Error(string msg, string src, string tag)
        {
            this.message = msg;
            this.source = src;
            this.target = tag;
        }
    }
}
