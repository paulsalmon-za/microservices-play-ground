using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Abstractions
{
    public class SubscriptionRegistration
    {
        private readonly ConcurrentDictionary<string, ListenConfig> _config;
        private readonly IInformationNameResolver _resolver;
        private readonly IDynamicConverter _converter;
        public SubscriptionRegistration(
            IInformationNameResolver resolver,
            IDynamicConverter converter
            )
        {
            _config = new ConcurrentDictionary<string, ListenConfig>();
            _resolver = resolver;
            _converter = converter;
        }

        internal void SubscribeCommand<T>()
            where T : ICommand
        {
            var type = typeof(ICommandHandler<T>);
            var config = new ListenConfig();
            config.Handler = type;
            config.ReflectionType = typeof(T);
            config.Map = (IDispatchRequest r) => _converter.ToObject<T>(r.Data);
            var name = _resolver.Resolve(config.ReflectionType);
            _config.AddOrUpdate(ResolveKey(name, Types.Command), config, (t, old)=> config);
        }

        internal void SubscribeResult<T>()
            where T : IInformative
        {
            var type = typeof(IInformativeHandler<T>);
            var config = new ListenConfig();
            config.Handler = type;
            config.ReflectionType = typeof(T);
            config.Map = (IDispatchRequest r) => _converter.ToObject<T>(r.Data);
            var name = _resolver.Resolve(config.ReflectionType);
            _config.AddOrUpdate(ResolveKey(name, Types.Result), config, (t, old)=> config);
        }

        public bool TryGetConfig(string name, Types type, out ListenConfig config) 
        {
            var key = ResolveKey(name, type);
            var hasKey = _config.TryGetValue(key, out config);
            return hasKey;
        }

        private string ResolveKey(string name, Types type)
        {
            return $"{type}.{name}";
        }

    }
}