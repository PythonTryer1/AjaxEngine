using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace AjaxEngine.Utils
{
    public static class Resources
    {
        private static Assembly assembly = Assembly.GetExecutingAssembly();
        private static Dictionary<string, string> textCache = new Dictionary<string, string>();
        public static string GetTextResource(string name)
        {
            if (!textCache.ContainsKey(name))
            {
                Stream stream = assembly.GetManifestResourceStream(name);
                StreamReader reader = new StreamReader(stream);
                textCache[name] = reader.ReadToEnd();
            }
            return textCache[name];
        }
        public static string Jquery
        {
            get
            {
                return GetTextResource("AjaxEngine.Scripts.jquery.js");
            }
        }
        public static string PageCore
        {
            get
            {
                return GetTextResource("AjaxEngine.Scripts.page-core.js");
            }
        }
        public static string ServiceCore
        {
            get
            {
                return GetTextResource("AjaxEngine.Scripts.service-core.js");
            }
        }
    }
}
