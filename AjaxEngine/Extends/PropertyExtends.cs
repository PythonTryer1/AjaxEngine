using System.Reflection;

namespace AjaxEngine.Extends
{
    public static class PropertyExtends
    {
        public static T GetAttribute<T>(this PropertyInfo property)
        {
            object[] attributes = property.GetCustomAttributes(true);
            foreach (object att in attributes)
            {
                if (att is T)
                    return (T)att;
            }
            return default(T);
        }

    }
}
