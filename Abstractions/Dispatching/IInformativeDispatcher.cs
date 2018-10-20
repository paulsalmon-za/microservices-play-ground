using System.Threading.Tasks;

namespace Abstractions
{
    public interface IInformativeDispatcher<T>
        where T: IInformative
    {
        Task<IDispatchResponse> Dispatch(IDispatchRequest information);
    }
}