using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AjaxEngine.Serializes;

namespace AjaxEngine
{
    public class Gloabl
    {
        private static ISerializer _Serializer = null;
        public static ISerializer Serializer
        {
            get
            {
                if (_Serializer == null)
                    _Serializer = new JsonSerializer();
                return _Serializer;
            }
            set
            {
                _Serializer = value;
            }
        }
    }
}
