using System;
using System.Text;
using System.Threading;
using NLog;

namespace nats_tools
{
    public class SendCommand : AbstractSendCommand<SendOptions>
    {
        private new static Logger Logger { get; } = LogManager.GetCurrentClassLogger();
        public SendCommand() : base(Logger)
        {
        }

        protected override void DoWork(string msgTxt, byte[] data)
        {
            Logger.Info($"Send: {Options.Subject} => '{msgTxt}' - {data.Length} bytes");
            Options.Connection.Publish(Options.Subject, data);
        }
    }
    
    public abstract class AbstractSendCommand<T> : AbstractNatsCommand<T> where T : SendOptions 
    {
        int NbMessages { get; set; }
        protected abstract void DoWork(string msgTxt, byte[] data);
        
        protected AbstractSendCommand(Logger logger) : base(logger)
        {
        }

        public override int Run()
        {
            NbMessages = 0;
            DateTime end = DateTime.Now.AddSeconds(Options.Wait);

            while (   (Options.Count > 0 && NbMessages < Options.Count)
                   || (Options.Wait > 0 && DateTime.Now < end))
            {
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

                DoWork(msgTxt, data);

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
