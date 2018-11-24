using System;
using System.Text;

namespace nats_tools
{
    internal class SendCommand : AbstractNatsCommand<SendOptions>
    {
        public override int Run()
        {
            byte[] data = Encoding.Default.GetBytes(Options.Message);
            Console.WriteLine($"Send: {Options.Subject} => [{Options.Message}]");
            Options.Connection.Publish(Options.Subject, data);
            Options.Connection.Close();
            return 0;
        }
    }
}
