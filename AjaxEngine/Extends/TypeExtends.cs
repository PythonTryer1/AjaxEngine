using System;
using System.Collections.Generic;
using System.Text;
using AjaxEngine.Reflection;
using System.Reflection;

namespace AjaxEngine.Extends
{
    public static class TypeExtends
    {
        public static T GetAttribute<T>(this Type type)
        {
            object[] attributes = type.GetCustomAttributes(true);
            foreach (object att in attributes)
            {
                if (att is T)
                    return (T)att;
            }
            return default(T);
        }
    }
}
