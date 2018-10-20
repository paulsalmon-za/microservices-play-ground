using System.Threading.Tasks;

namespace Abstractions
{
    public interface ICommandDispatcher<T>
        where T: ICommand
    {
        Task<IDispatchResponse> Dispatch(IDispatchRequest command);
    }

}