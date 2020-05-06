using CommandLine;

namespace nats_tools
{
    public abstract class AbstractSendOptions : AbstractNatsOptions
    {
        [Option('s', "subject", Required = true, HelpText = "Nats message subject")]
        public string Subject { get; set; }

        [Option('m', "msg", Required = true, HelpText = "Nats message content. Variables: {n} : message number, {time} : local time (format hh:mm:ss.fff)")]
        public string Message { get; set; }

        [Option('p', "period", HelpText = "Time in ms between messages", Default = 1000)]
        public int Period { get; set; }

        [Option('l', "length", HelpText = "Message length. If -m is specified, its length is filled or reduced as needed. 0 means the message is not modified", Default = 0)]
        public int Length { get; set; }

        [Option('c', "count", HelpText = "Exits after n messages", Default = 1)]
        public int Count { get; set; }

        [Option('w', "wait", HelpText = "Waits for n seconds then exits", Default = -1)]
        public int Wait { get; set; } = -1;
    }
}