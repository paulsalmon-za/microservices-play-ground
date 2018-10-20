using System;
using System.Linq;
using System.Threading.Tasks;

namespace Abstractions
{
    public interface IDispatchService
    {
        Task<IDispatchResponse> DispatchCommand<T>(T command)
            where T: ICommand;
        Task<IDispatchResponse> DispatchInformation<T>(T informative)
            where T: IInformative;
    }
    public class DispatchService : IDispatchService
    {
        private readonly IDependencyResolver _resolver;
        private readonly IInformationNameResolver _nameResolver;

        private readonly ICorrelationService _correlation;
        public DispatchService(
            IDependencyResolver resolver, 
            ICorrelationService correlation,
            IInformationNameResolver nameResolver
            )
        {
            _resolver = resolver;
            _correlation = correlation;
            _nameResolver = nameResolver;
        }
        public async Task<IDispatchResponse> DispatchCommand<T>(T command) 
            where T: ICommand
        {
            command.CorrelationId = _correlation.GenerateIdIfNull(command.CorrelationId);
            command.TimeStamp = DateTime.Now;

            var request = GetRequestMessage(command);

            var requestDispatcher = _resolver.Resolve<ICommandDispatcher<T>>();
            return await requestDispatcher.Dispatch(request);
        }

        public async Task<IDispatchResponse> DispatchInformation<T>(T informative) 
            where T: IInformative
        {
            informative.CorrelationId = _correlation.GenerateIdIfNull(informative.CorrelationId);
            informative.TimeStamp = DateTime.Now;

            var request = GetRequestMessage(informative);

            var requestDispatcher = _resolver.Resolve<IInformativeDispatcher<T>>();
            return await requestDispatcher.Dispatch(request);
        }


        private IDispatchRequest GetRequestMessage<T>(T command)
            where T: IInformation
        {
            return new DispatchRequest()
            {
                Name = _nameResolver.Resolve<T>(),
                Data = command,
                Type = ResolveType<T>()
            };
        }

        private Types ResolveType<T>()
        {
            var interfaces = typeof(T).GetInterfaces();
            if (interfaces.Contains(typeof(IFuture)))
            {
                return Types.Future;
            }

            if (interfaces.Contains(typeof(IResult)))
            {
                return Types.Result;
            }

            if (interfaces.Contains(typeof(ICommand)))
            {
                return Types.Command;
            }

            throw new ArgumentException($"Could not resolve Information Type for {typeof(T).Name}");
        }
    }
}