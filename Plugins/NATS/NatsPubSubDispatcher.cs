using System;
using System.Threading.Tasks;
using Abstractions;

namespace Plugins.NATS
{
    public class NatsPubSubDispatcher<T> : NatsDispatcherBase<T>, IInformativeDispatcher<T>
        where T : IInformative
    {
        public NatsPubSubDispatcher(
            NatsConfig config,
            ILoggerService<NatsDispatcherBase<T>> logger
            ) : base(logger, config)
        {
        }
        
    }
}