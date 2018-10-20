namespace Abstractions
{
    public interface IDispatchRequest
    {
        Types Type { get; }
        string Name { get; }
        string ResponseChannel { get; }
        dynamic Data { get; }
    }

    public class DispatchRequest : IDispatchRequest
    {
        public Types Type { get; set; }
        public string Name { get; set; }

        public string ResponseChannel { get; set; }

        public dynamic Data { get; set; }
    }

    public interface IDispatchResponse
    {
        Types Type { get; }
        string Name { get; }
        string ResponseChannel { get; }
        dynamic Data { get; }
    }

    public class DispatchResponse : IDispatchResponse
    {
        public Types Type { get; set; }
        public string Name { get; set; }
        public string ResponseChannel { get; set;}
        public dynamic Data { get; set; }
    }
}