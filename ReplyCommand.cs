using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using NATS.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace nats_tools
{
    internal class ReplyCommand : AbstractNatsCommand<ReplyOptions>
    {
        private new static Logger Logger { get; }= LogManager.GetCurrentClassLogger();
        private int NbMessages { get; set; }
        
        public ReplyCommand() : base(Logger)
        {
        }

        public override int Run()
        {
            if (Options.Subject == null)
            {
                Options.Subject = ">";
            }
            Logger.Info($"Reply: {Options.Subject}");
            Options.Connection.SubscribeAsync(Options.Subject, OnMessage);

            if (Options.Count <= 0 && Options.Wait <= 0)
            {
                return 0;
            }

            DateTime end = DateTime.Now.AddSeconds(Options.Wait);
            NbMessages = Options.Count;
            while ((Options.Count > 0 && NbMessages > 0) && (Options.Wait > 0 && DateTime.Now < end))
            {
                Thread.Sleep(100);
            }
            Options.Connection.Close();
            return 0;
        }

        private void OnMessage(object sender, MsgHandlerEventArgs e)
        {
            NbMessages--;
            Logger logger = LogManager.GetLogger(e.Message.Subject);
            if (e.Message.Reply == null)
            {
                return;
            }
            var msg = Options.Message;
            msg = msg.Replace("{time}", DateTime.Now.ToString("HH:mm:ss.fff"));
            msg = msg.Replace("{n}", NbMessages.ToString());

            byte[] data;
            if (Options.Length > 0)
            {
                data = new byte[Options.Length];
                Encoding.Default.GetBytes(msg, 0, Math.Min(data.Length, msg.Length), data, 0);
            }
            else
            {
                data = Encoding.Default.GetBytes(msg);
            }

            string msgTxt = msg;
            if (Options.Length > 0 && msg.Length > Options.Length)
            {
                msgTxt = msgTxt.Substring(0, Options.Length);
            }
            if (msgTxt.Length > 80)
            {
                msgTxt = msgTxt.Substring(0, 80) + "...";
            }
            Options.Connection.Publish(e.Message.Reply, data);

            logger.Info($"Reply: {e.Message.Data.Length} - '{Options.Message}'");
        }
    }
}