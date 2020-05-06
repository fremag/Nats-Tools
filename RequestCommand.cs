using System;
using System.Text;
using NLog;

namespace nats_tools
{
    public class RequestCommand : AbstractSendCommand<RequestOptions>
    {
        private new static Logger Logger { get; } = LogManager.GetCurrentClassLogger();
        public RequestCommand() : base(Logger)
        {
        }

        protected override void DoWork(string msgTxt, byte[] data)
        {
            Logger.Info($"Request: {Options.Subject} => {data.Length} bytes");
            try
            {
                var msgReply = Options.Connection.Request(Options.Subject, data, Options.Timeout);
                var replyTxt = Encoding.Default.GetString(msgReply.Data);
                Logger.Info($"Reply: '{replyTxt}' - {data.Length} bytes");
            }
            catch (TimeoutException)
            {
                Logger.Error("Timeout !");
            }
        }
    }
}