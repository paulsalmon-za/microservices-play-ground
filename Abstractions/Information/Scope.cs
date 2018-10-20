using System;

namespace Abstractions
{
    public enum Scope : UInt16 
    {
        Block = 0,
        Public = 1,
        Internal = 2,
        Any = 3
    }
}