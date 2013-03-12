using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace AjaxEngine.Reflection
{
    public class ParameterCache
    {
        private static object m_mutex = new object();
        private static Dictionary<MethodInfo, ParameterInfo[]> ParameterInfoListCache = new Dictionary<MethodInfo, ParameterInfo[]>();
        public static ParameterInfo[] GetPropertyInfo(MethodInfo methodInfo)
        {
            ParameterInfo[] pList;
            if (ParameterInfoListCache.TryGetValue(methodInfo, out pList))
                return pList;
            ParameterInfoListCache[methodInfo] = methodInfo.GetParameters();
            return ParameterInfoListCache[methodInfo];
        }

    }
}
