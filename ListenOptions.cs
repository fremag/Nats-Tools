using System.Collections.Generic;
using CommandLine;

namespace nats_tools
{
    [Verb("listen", HelpText = "Listen to Nats subject")]
    class ListenOptions : AbstractNatsOptions
    {
        [CommandLine.Option('s', "subjects", HelpText = "Nats subjects to listen")]
        public IEnumerable<string> Subjects { get; set; }

        [CommandLine.Option('c', "count", HelpText = "Exists after c messages", Default = -1)]
        public int Count { get; set; } = -1;

        [CommandLine.Option('t', "time", HelpText = "Exits after t seconds", Default = -1)]
        public int TimeS { get; set; } = -1;
    }
}
