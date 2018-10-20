using System;

namespace Abstractions
{
    public interface IDependencyResolver
    {
        T Resolve<T>();
        object Resolve(Type type);
    }
}