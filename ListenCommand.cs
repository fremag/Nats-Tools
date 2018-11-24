using System;
using System.Linq;
using NATS.Client;

namespace nats_tools
{
    internal class ListenCommand : AbstractNatsCommand<ListenOptions>
    {
        public override int Run()
        {
            if (Options.Subjects == null || !Options.Subjects.Any())
            {
                Options.Subjects = new[] { ">" };
            }
            foreach (var subject in Options.Subjects)
            {
                Console.WriteLine($"Listen: {subject}");
                Options.Connection.SubscribeAsync(subject, OnMessage);
            }
            return 0;
        }

        private static void OnMessage(object sender, MsgHandlerEventArgs e)
        {
            Console.WriteLine($"{DateTime.Now:HH:mm:ss}\t{e.Message.Subject}\t{e.Message.Data.Length}");
        }
    }
}