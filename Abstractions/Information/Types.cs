using System;

namespace Abstractions
{
    public enum Types: UInt16
    {
        Block = 0,
        Command = 1,
        Future = 2,
        CommandFuture = 3,
        Result = 4,
        CommandResult = 5,
        Any = 6

    }
}