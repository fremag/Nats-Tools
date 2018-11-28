using System;
using System.Linq;
using System.Text;
using NATS.Client;
using NLog;

namespace nats_tools
{
    internal class ListenCommand : AbstractNatsCommand<ListenOptions>
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        public override int Run()
        {
            if (Options.Subjects == null || !Options.Subjects.Any())
            {
                Options.Subjects = new[] { ">" };
            }
            foreach (var subject in Options.Subjects)
            {
                logger.Info($"Listen: {subject}");
                Options.Connection.SubscribeAsync(subject, OnMessage);
            }
            
            return 0;
        }

        private static void OnMessage(object sender, MsgHandlerEventArgs e)
        {
            Logger logger = LogManager.GetLogger(e.Message.Subject);
            string msgTxt = Encoding.Default.GetString(e.Message.Data);
            logger.Info($"{e.Message.Data.Length} - '{msgTxt}'");
        }
    }
}