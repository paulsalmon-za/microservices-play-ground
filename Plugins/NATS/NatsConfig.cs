namespace Plugins.NATS
{
    public class NatsConfig
    {
        public string Host { get; set; }
        public int? Port { get; set; }

        public string WorkerGroup { get; set;}
        public string SubjectPattern { get; set;}
    }
}