using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace AjaxEngine.Reflection
{
    public class TypeCache
    {
        private static object m_mutex = new object();
        private static Dictionary<string, Type> m_cache = new Dictionary<string, Type>();

        public static Type GetType(string typeFullName)
        {
            Type type;
            if (m_cache.TryGetValue(typeFullName, out type))
            {
                return type;
            }

            lock (m_mutex)
            {
                Assembly[] allAssembly = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly ass in allAssembly)
                {
                    type = ass.GetType(typeFullName);
                    if (type != null)
                        break;
                }
                m_cache[typeFullName] = type;
                return type;
            }
        }
    }
}
