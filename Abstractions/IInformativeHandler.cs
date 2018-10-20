using System.Threading.Tasks;

namespace Abstractions
{
    public interface IInformativeHandler<T>
        where T : IInformative
    {
        Task Dessiminate(T information);
    }

    public interface ICommandResultHandler<T, TValue> : IInformativeHandler<Completed<T, TValue>>
        where T : ICommand
    {
    }

    public interface ICommandResultHandler<T> : IInformativeHandler<Completed<T>>
        where T : ICommand
    {
    }
}