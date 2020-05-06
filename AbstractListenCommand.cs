using System;
using System.Linq;
using System.Threading;
using NATS.Client;
using NLog;

namespace nats_tools
{
    public abstract class AbstractListenCommand<T> : AbstractNatsCommand<T> where T : AbstractListenOptions
    {
        protected AbstractListenCommand(Logger logger) : base(logger)
        {
        }

        protected int NbMessages { get; set; }
        protected abstract void OnMessage(object sender, MsgHandlerEventArgs e);

        public override int Run()
        {
            if (Options.Subjects == null || !Options.Subjects.Any())
            {
                Options.Subjects = new[] { ">" };
            }
            
            foreach (var subject in Options.Subjects)
            {
                Logger.Info($"Listen: {subject}");
                Options.Connection.SubscribeAsync(subject, OnMessage);
            }

            DateTime end = DateTime.Now.AddSeconds(Options.Wait);
            NbMessages = Options.Count;
            while (
                (Options.Count <= 0 || NbMessages > 0)
                && (Options.Wait < 0 || DateTime.Now < end))
            {
                Thread.Sleep(100);
            }
            Options.Connection.Close();
            return 0;
        }
    }
}