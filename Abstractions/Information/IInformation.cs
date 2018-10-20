using System;

namespace Abstractions
{
    public interface IInformation
    {
        string CorrelationId { get; set; }
        DateTime TimeStamp { get; set; }
    }

    

    public interface IEvent: IInformation
    {
        string EventId { get; }
    }

    public interface ICommand: IEvent
    {
        
    }

    public interface IInformative: IEvent
    {

    }

    public interface IFuture: IInformative
    {
        DateTime? ETA { get; }
    }

    public interface IResult: IInformative
    {
        DateTime? Delivered { get; }
    }

    public interface ICommandFuture : ICommand, IFuture
    {

    }

    public interface ICommandResult<T> : ICommand, IResult
        where T : ICommand
    {
        bool? Accomplished { get; }
    }

    public interface ICommandResultFuture : ICommand, IResult, IFuture
    {
        
    }

}