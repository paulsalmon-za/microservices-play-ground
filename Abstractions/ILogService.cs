using System;

namespace Abstractions
{
    public interface ILoggerService<T>
    {
        void LogInformation(string message);
        void LogError(Exception exception, string message);
    }
}