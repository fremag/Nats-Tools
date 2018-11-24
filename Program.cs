using System;
using System.Collections.Generic;
using CommandLine;
using NATS.Client;

namespace nats_tools
{
    class NatsOptions
    {
        [CommandLine.Option('n', "nats", HelpText="Nats server url, default: nats://localhost:4222")]
        public string NatsUrl { get; set; } = "nats://localhost:4222";
        public IConnection Connection { get; private set; }

        public void Start()
        {
            NATS.Client.ConnectionFactory factory = new NATS.Client.ConnectionFactory();
            Connection = factory.CreateConnection(NatsUrl);
        }
    }

    [Verb("listen", HelpText = "Listen to NATS subject")]
    class ListenOptions : NatsOptions
    {
        [CommandLine.Option('s', "subjects", HelpText="Nats subjects to listen", Default=">")]
        public IEnumerable<string> Subjects { get; set; } = new string[] {">"};

        [CommandLine.Option('c', "count", HelpText="Exists after c messages", Default=-1)]
        public int Count { get; set; } = -1;

        [CommandLine.Option('t', "time", HelpText="Exits after t seconds", Default=-1)]
        public int TimeS { get; set; } = -1;
    }

    public class Program
    {
        static int Main(string[] args)
        {
            return CommandLine.Parser.Default.ParseArguments<ListenOptions>(args)
              .MapResult(
                (ListenOptions opts) => ListenSubject(opts),
                errs => 1);
        }

        static int ListenSubject(ListenOptions options)
        {
            options.Start();
            foreach (var subject in options.Subjects)
            {
                Console.WriteLine($"Listen: {subject}");
                options.Connection.SubscribeAsync(subject, OnMessage);
            }
            return 0;
        }

        private static void OnMessage(object sender, MsgHandlerEventArgs e)
        {
        }
    }
}
