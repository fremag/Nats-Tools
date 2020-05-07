using CommandLine;

namespace nats_tools
{
    [Verb("stat", HelpText = "Statistics on received Nats messages")]
    public class StatOptions : AbstractListenOptions
    {
        [Option('p', "period", HelpText = "Time in ms between stats print", Default = 5000)]
        public int Period { get; set; }
    }
}