using System;
using System.Threading.Tasks;
using Abstractions;

namespace RacingConsumer
{
    public class CalculateSumCommand : ICommand
    {
        public string EventId { get; set; }

        public string CorrelationId { get; set; }

        public DateTime TimeStamp { get; set; }

        public int A { get; set;}

        public int B { get; set;}

    }

    public class CalculateSumCommandHandler : ICommandHandler<CalculateSumCommand>
    {
        public async Task<ICommandResult<CalculateSumCommand>> Execute(CalculateSumCommand command)
        {
            return await Result.Completed(command, command.A + command.B);
        }
    }

    public class CalculateSumResultHandler : ICommandResultHandler<CalculateSumCommand, int>
    {
        private readonly ILoggerService<CalculateSumResultHandler> _logger;
        public CalculateSumResultHandler(ILoggerService<CalculateSumResultHandler> logger)
        {
            _logger = logger;
        }
        public async Task Dessiminate(Completed<CalculateSumCommand, int> result)
        {
            _logger.LogInformation($"{result.Accomplished}");

            await Task.CompletedTask;
        }
    }
}