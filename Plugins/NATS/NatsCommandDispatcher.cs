using Abstractions;

namespace Plugins.NATS
{
    public class NatsCommandDispatcher<T> : NatsDispatcherBase<T>, ICommandDispatcher<T>
        where T : ICommand
    {
        public NatsCommandDispatcher(
            NatsConfig config,
            ILoggerService<NatsDispatcherBase<T>> logger
            ) : base(logger, config)
        {
        }
        
    }
}