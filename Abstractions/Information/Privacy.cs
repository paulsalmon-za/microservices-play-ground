using System;

namespace Abstractions
{
    public enum Privacy : UInt16
    {
        Block = 0,
        Application = 1,
        User = 2,
        Request = 3,
        Any = 10
    }
}