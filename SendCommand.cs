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
}
