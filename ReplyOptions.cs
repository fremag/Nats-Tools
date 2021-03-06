using CommandLine;

namespace nats_tools
{
    [Verb("reply", HelpText = "Reply to Nats requests")]
    public class ReplyOptions : AbstractListenOptions
    {
        [Option('m', "msg", Required = true, HelpText = "Nats message content. Variables: {n} : message number, {time} : local time (format hh:mm:ss.fff)")]
        public string Message { get; set; }

        [Option('l', "length", HelpText = "Message length. If -m is specified, its length is filled or reduced as needed. 0 means the message is not modified", Default = 0)]
        public int Length { get; set; }
    }
}
