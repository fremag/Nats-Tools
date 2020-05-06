using CommandLine;

namespace nats_tools
{
    [Verb("request", HelpText = "Send Nats request messages")]
    public class RequestOptions : AbstractSendOptions 
    {
        [Option('t', "timeout", HelpText = "Time in milliseconds to wait for a reply", Default = 10000)]
        public int Timeout { get; set; }
    }
}