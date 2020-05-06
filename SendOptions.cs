using CommandLine;

namespace nats_tools
{
    [Verb("send", HelpText = "Send Nats messages")]
    public class SendOptions : AbstractSendOptions
    {
    }
}
