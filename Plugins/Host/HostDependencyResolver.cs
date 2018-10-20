using System;
using Abstractions;

namespace Plugins.Host.AspNetCore
{
    public class HostDependencyResolver : IDependencyResolver
    {
        private readonly IServiceProvider _provider;
        public HostDependencyResolver(IServiceProvider provider)
        {
            _provider = provider;
        }
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public object Resolve(Type type)
        {
            return _provider.GetService(type);
        }
    }
}