using System.Collections.Generic;
using CommandLine;

namespace nats_tools
{
    public class AbstractListenOptions : AbstractNatsOptions
    {
        [Option('s', "subjects", HelpText = "Nats subjects to listen")]
        public IEnumerable<string> Subjects { get; set; }

        [Option('c', "count", HelpText = "Exits after n messages", Default = -1)]
        public int Count { get; set; } = -1;

        [Option('w', "wait", HelpText = "Waits for n seconds then exits", Default = -1)]
        public int Wait { get; set; } = -1;
    }
}