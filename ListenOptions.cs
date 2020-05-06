using System.Collections.Generic;
using CommandLine;

namespace nats_tools
{
    [Verb("listen", HelpText = "Listen to Nats subject")]
    public class ListenOptions : AbstractListenOptions
    {
        [Option('t', "tokens", HelpText = "JSON tokens to extract")]
        public IEnumerable<string> Tokens { get; set; }

        [Option('d', "delimiter", HelpText = "Token output delimiter", Default = ",")]
        public string Delimiter { get; set; }

        [Option('j', "json", HelpText = "JSON pretty print", Default = false)]
        public bool Json { get; set; } = false;
    }
}
