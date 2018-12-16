using System;
using System.Text;
using System.Threading;
using NLog;

namespace nats_tools
{
    internal class SendCommand : AbstractNatsCommand<SendOptions>
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        int NbMessages { get; set; }
        public override int Run()
        {
            NbMessages = 0;
            DateTime end = DateTime.Now.AddSeconds(Options.Wait);
            while ((Options.Count > 0 && NbMessages < Options.Count) || (Options.Wait > 0 && DateTime.Now < end))
            {
                var msg = Options.Message;
                msg = msg.Replace("{time}", DateTime.Now.ToString("HH:mm:ss.fff"));
                msg = msg.Replace("{n}", NbMessages.ToString());
                byte[] data = Encoding.Default.GetBytes(msg);
                string msgTxt = msg;
                if (msgTxt.Length > 80)
                {
                    msgTxt = msgTxt.Substring(80) + "...";
                }
                logger.Info($"Send: {Options.Subject} => '{msgTxt}'");
                Options.Connection.Publish(Options.Subject, data);
                NbMessages++;
                if (Options.Period > 0)
                {
                    Thread.Sleep(Options.Period);
                }
            }

            Options.Connection.Close();
            return 0;
        }
    }
}
