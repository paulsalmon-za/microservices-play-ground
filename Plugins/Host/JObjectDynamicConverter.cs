using System;
using Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Plugins.Host.AspNetCore
{
    public class JObjectDynamicConverter : IDynamicConverter
    {
        public dynamic ToDynamic(object objectValue)
        {
            if (objectValue == null)
                return null;

            return JObject.FromObject(objectValue);
        }

        public T ToObject<T>(dynamic dynamicValue)
        {
            if (dynamicValue == null)
                return default(T);

            return ((JObject)dynamicValue).ToObject<T>();
        }

        public object ToObject(dynamic dynamicValue, Type type)
        {
            if (dynamicValue == null)
                return null;

            return ((JObject)dynamicValue).ToObject(type);
        }
    }
}