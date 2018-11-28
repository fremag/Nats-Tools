using System;
using System.Text;
using NLog;

namespace nats_tools
{
    internal class SendCommand : AbstractNatsCommand<SendOptions>
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        public override int Run()
        {
            byte[] data = Encoding.Default.GetBytes(Options.Message);
            logger.Info($"Send: {Options.Subject} => [{Options.Message}]");
            Options.Connection.Publish(Options.Subject, data);
            Options.Connection.Close();
            return 0;
        }
    }
}
