using CommandLine;

namespace nats_tools
{
    [Verb("send", HelpText = "Send Nats message")]
    class SendOptions : AbstractNatsOptions
    {
        [CommandLine.Option('s', "subject", HelpText = "Nats message subject")]
        public string Subject { get; set; }

        [CommandLine.Option('m', "msg", HelpText = "Nats message content")]
        public string Message { get; set; }
    }
}
