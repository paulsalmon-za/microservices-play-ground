using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NATS.Client;
using Newtonsoft.Json;
using Abstractions;


namespace Plugins.NATS
{
    public class NatsHostedService : IHostedService, IDisposable
    {
        private NatsConfig _config;
        private IEncodedConnection _connection;
        private IAsyncSubscription _subscriptions;
        private readonly SubscriptionRegistration _registration;
        private readonly IDispatchService _dispatchService;

        private readonly IDependencyResolver _resolver;
        private readonly ILoggerService<NatsHostedService> _logger;

        public NatsHostedService(
            SubscriptionRegistration registration,
            IDependencyResolver resolver,
            IDispatchService dispatchService,
            NatsConfig config,
            ILoggerService<NatsHostedService> logger
            )
        {
            _registration = registration;
            _resolver = resolver;
            _dispatchService = dispatchService;
            _config = config;
            _logger = logger;
        }
        public void Dispose()
        {
            if (_subscriptions != null) 
            {
                _subscriptions.Dispose();
                _subscriptions = null;
            }

            if (_connection != null) 
            {
                _connection.Dispose();
                _connection = null;
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var opts = ConnectionFactory.GetDefaultOptions();
            
            opts.Url = $"{_config.Host}:{_config.Port ?? 4222}";
            _connection = new ConnectionFactory().CreateEncodedConnection(opts);
            _connection.OnDeserialize = jsonDeserializer;
            _connection.OnSerialize = jsonSerializer;

            var pattern = _config.SubjectPattern ?? "*";
            var workerGroup = _config.WorkerGroup ?? "WORKERGROUP";
            _logger.LogInformation($"Pattern: {pattern}");
            _logger.LogInformation($"Group: {workerGroup}");
            _subscriptions = _connection.SubscribeAsync(pattern, workerGroup, HandleMessage);
            _subscriptions.Start();

            return Task.CompletedTask;
        }

        private void HandleMessage(object sender, EncodedMessageEventArgs args)
        {
            _logger.LogInformation("Message received");
            ListenConfig config;
            GetMessageInfoFromSubject(args.Message.Subject, out string name, out Types type);
            if(!_registration.TryGetConfig(name, type, out config) || config == null)
            {
                return;
            }

            var request = args.ReceivedObject as DispatchRequest;
            var information = config.Map(request);
            var methodName = type == Types.Command ? "ExecuteCommand" : "Dessiminate";

            var method = typeof(NatsHostedService).GetMethod(methodName);
            var genMethod = method.MakeGenericMethod(new [] { information.GetType() } );
            var task = genMethod.Invoke(this, new [] { information }) as Task;
            
        }

        public async Task<IDispatchResponse> ExecuteCommand<T>(T command)
            where T : ICommand
        {
            var handler = _resolver.Resolve<ICommandHandler<T>>();
            var result = await handler.Execute(command);
            return await _dispatchService.DispatchInformation(result);
        }

        public async Task Dessiminate<T>(T information)
            where T : IInformative
        {
            var type = typeof(T);
            _logger.LogInformation($"Dessiminating message {type.Name}");
            var handler = _resolver.Resolve<IInformativeHandler<T>>();
            await handler.Dessiminate(information);
        }

        private void GetMessageInfoFromSubject(string subject, out string name, out Types type)
        {
            if (string.IsNullOrEmpty(subject))
                throw new ArgumentNullException(nameof(subject));

            var sections = subject.Split('.');

            if (sections.Length < 2)
                throw new ArgumentException($"{nameof(subject)} not in correct format");
            
            name = sections[1];
            
            type = Types.Block;
            switch(sections[0].ToLower()) {
                case "res":
                    type = Types.Result;
                    break;
                case "cmd":
                    type = Types.Command;
                    break;
                case "fut":
                    type = Types.Future;
                    break;
                default:
                    break;

            }
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

        public Task StopAsync(CancellationToken cancellationToken)
        {

            return Task.CompletedTask;
        }
    }
}