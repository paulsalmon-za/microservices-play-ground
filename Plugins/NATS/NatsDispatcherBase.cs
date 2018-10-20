using System;
using NATS.Client;
using Abstractions;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;

namespace Plugins.NATS
{
    public abstract class NatsDispatcherBase<T>
        where T : IInformation
    {
        private readonly NatsConfig _config;
        private readonly ILoggerService<NatsDispatcherBase<T>> _logger;
        public NatsDispatcherBase(
            ILoggerService<NatsDispatcherBase<T>> logger,
            NatsConfig config
            )
        {
            _config = config;
            _logger = logger;
        }
        public async Task<IDispatchResponse> Dispatch(IDispatchRequest commandRequest)
        {
            Options opts = ConnectionFactory.GetDefaultOptions();
            opts.Url = $"{_config.Host}:{_config.Port ?? 4222}";

            using (IEncodedConnection c = new ConnectionFactory().CreateEncodedConnection(opts))
            {   
                c.OnSerialize = jsonSerializer;
                c.OnDeserialize = jsonDeserializer;
                
                var subject = $"{ResolveTypePrefix(commandRequest.Type)}.{commandRequest.Name}";
                _logger.LogInformation($"Subject: {subject}");
                c.Publish(subject, commandRequest);
            }

            return await Task.FromResult(new DispatchResponse() {
                Name = commandRequest.Name,
                Data = commandRequest.Data,
                Type = commandRequest.Type,
                ResponseChannel = commandRequest.ResponseChannel
            });
        }

        private string ResolveTypePrefix(Types type)
        {
            if (type == Types.Command) {
                return "cmd";
            }

            if (type == Types.Result) {
                return "res";
            }

            if (type == Types.Result) {
                return "fut";
            }

            throw new ArgumentException(nameof(type));
        }

        internal object jsonDeserializer(byte[] buffer)
        {
            var json = Encoding.UTF8.GetString(buffer);
            return JsonConvert.DeserializeObject<DispatchRequest>(json);
        }

        internal byte[] jsonSerializer(object obj)
        {
            if (obj == null)
                return null;

            var json = JsonConvert.SerializeObject(obj);

            return Encoding.UTF8.GetBytes(json);
        }

        
    }
}