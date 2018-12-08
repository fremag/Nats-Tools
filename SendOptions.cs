using CommandLine;

namespace nats_tools
{
    [Verb("send", HelpText = "Send Nats message")]
    class SendOptions : AbstractNatsOptions
    {
        [Option('s', "subject",Required=true, HelpText = "Nats message subject")]
        public string Subject { get; set; }

        [Option('m', "msg",Required=true, HelpText = "Nats message content")]
        public string Message { get; set; }
    }
}
