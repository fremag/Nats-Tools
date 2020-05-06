using CommandLine;

namespace nats_tools
{
    [Verb("request", HelpText = "Send Nats request messages")]
    public class RequestOptions : SendOptions 
    {
        [Option('r', "request", HelpText = "Sends a request and waits for a reply", Default = false)]
        public bool Request { get; set; }

        [Option('t', "timeout", HelpText = "Time in milliseconds to wait for a reply", Default = 10000)]
        public int Timeout { get; set; }
    }
}