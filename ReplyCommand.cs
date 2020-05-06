using System;
using System.Text;
using NATS.Client;
using NLog;

namespace nats_tools
{
    internal class ReplyCommand : AbstractListenCommand<ReplyOptions>
    {
        private new static Logger Logger { get; }= LogManager.GetCurrentClassLogger();
        
        public ReplyCommand() : base(Logger)
        {
        }

        protected override void OnMessage(object sender, MsgHandlerEventArgs e)
        {
            NbMessages--;
            Logger logger = LogManager.GetLogger(e.Message.Subject);
            if (e.Message.Reply == null)
            {
                return;
            }
            var msg = Options.Message;
            msg = msg.Replace("{time}", DateTime.Now.ToString("HH:mm:ss.fff"));
            msg = msg.Replace("{n}", (Options.Count - NbMessages).ToString());

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

            logger.Info($"Reply: {e.Message.Data.Length} - '{msgTxt}'");
        }
    }
}