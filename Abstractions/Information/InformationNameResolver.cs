using System;
using System.Linq;

namespace Abstractions
{
    public interface IInformationNameResolver
    {
        string Resolve<T>();
        string Resolve(Type type);
    }

    public class SimpleInformationNameResolver : IInformationNameResolver
    {
        public string Resolve<T>()
        {
            return Resolve(typeof(T));
        }

        public string Resolve(Type type)
        {
            if (type.IsGenericType) {
                return ResolveGenericName(type);
            }
            return type.Name.ToLower().Replace("command", string.Empty);
        }

        private string ResolveGenericName(Type type)
        {
            var args = type.GetGenericArguments();
            return args[0].Name.ToLower();
        }
    }
}