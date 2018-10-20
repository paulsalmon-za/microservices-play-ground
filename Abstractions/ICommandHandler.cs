using System.Threading.Tasks;

namespace Abstractions
{
    public interface ICommandHandler<T>
        where T : ICommand
    {
        Task<ICommandResult<T>> Execute(T command);
    }
}