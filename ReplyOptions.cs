using CommandLine;

namespace nats_tools
{
    [Verb("reply", HelpText = "Reply to Nats requests")]
    class ReplyOptions : AbstractNatsOptions
    {
        [Option('s', "subject", Required = true, HelpText = "Nats message subject")]
        public string Subject { get; set; }

        [Option('m', "msg", Required = true, HelpText = "Nats message content. Variables: {n} : message number, {time} : local time (format hh:mm:ss.fff)")]
        public string Message { get; set; }

        [Option('c', "count", HelpText = "Exits after n requests", Default = -1)]
        public int Count { get; set; }

        [Option('w', "wait", HelpText = "Waits for n seconds then exits", Default = -1)]
        public int Wait { get; set; } = -1;

        [Option('l', "length", HelpText = "Message length. If -m is specified, its length is filled or reduced as needed. 0 means the message is not modified", Default = 0)]
        public int Length { get; set; }
    }
}
