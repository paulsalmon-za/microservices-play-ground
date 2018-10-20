using System;
using System.Threading.Tasks;

namespace Abstractions
{
    public class Completed<T> : ICommandResult<T>
        where T: ICommand
    {
        public string EventId { get; set; }

        public DateTime? Delivered { get; set; }

        public string CorrelationId { get; set; }

        public DateTime TimeStamp { get; set; }

        public bool? Accomplished => true;

        
    }

    public class Completed<T, TValue> : Completed<T>
        where T: ICommand
    {
        public TValue Value { get; set;}
        
    }

    public static class Result 
    {
        public static async Task<Completed<T>> Completed<T>(T command) 
            where T : ICommand
        {
            var result = new Completed<T>();
            result.EventId = command.EventId;
            result.CorrelationId = command.CorrelationId;
            result.TimeStamp = DateTime.Now;
            return await Task.FromResult(result);
        }

        public static async Task<Completed<T, TValue>> Completed<T, TValue>(T command, TValue value) 
            where T : ICommand
        {
            var result = new Completed<T,TValue>();
            result.EventId = command.EventId;
            result.CorrelationId = command.CorrelationId;
            result.TimeStamp = DateTime.Now;
            result.Value = value;
            return await Task.FromResult(result);
        }
    }
}