using NATS.Client;

namespace nats_tools
{
    public abstract class AbstractNatsOptions
    {
        [CommandLine.Option('n', "nats", HelpText = "Nats server url, default: nats://localhost:4222")]
        public string NatsUrl { get; set; } = "nats://localhost:4222";
        public IConnection Connection { get; private set; }

        public void Start()
        {
            NATS.Client.ConnectionFactory factory = new NATS.Client.ConnectionFactory();
            Connection = factory.CreateConnection(NatsUrl);
        }
    }
}
