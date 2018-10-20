using System;

namespace Abstractions
{
    public interface IDynamicConverter
    {
        T ToObject<T>(dynamic dynamicValue);
        object ToObject(dynamic dynamicValue, Type type);
        dynamic ToDynamic(object objectValue);

    }
}