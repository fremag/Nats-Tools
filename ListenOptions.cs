using System.Collections.Generic;
using CommandLine;

namespace nats_tools
{
    [Verb("listen", HelpText = "Listen to Nats subject")]
    class ListenOptions : AbstractNatsOptions
    {
        [Option('s', "subjects", HelpText = "Nats subjects to listen")]
        public IEnumerable<string> Subjects { get; set; }

        [Option('t', "tokens", HelpText = "JSON tokens to extract")]
        public IEnumerable<string> Tokens { get; set; }

        [Option('d', "delimiter", HelpText = "Token output delimiter", Default = ",")]
        public string Delimiter { get; set; }

        [Option('c', "count", HelpText = "Exists after c messages", Default = -1)]
        public int Count { get; set; } = -1;

        [Option('w', "wait", HelpText = "Waits for n seconds then exits", Default = -1)]
        public int Wait { get; set; } = -1;

        [Option('j', "json", HelpText = "JSON pretty print", Default = false)]
        public bool Json { get; set; } = false;
    }
}
