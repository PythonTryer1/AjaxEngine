using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AjaxEngine.Serializes
{
    public interface ISerializer
    {
        string Serialize(object obj);
        T Deserialize<T>(string text);
        object Deserialize(string text,Type type);
    }
}
