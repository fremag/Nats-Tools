using System;
using System.Linq;
using CommandLine;
using NLog;

namespace nats_tools
{
    public class Program
    {
        static int Main(string[] args)
        {
            
            return CommandLine.Parser.Default.ParseArguments<ListenOptions, SendOptions>(args)
              .MapResult(
                (ListenOptions opts) => RunCommand<ListenCommand, ListenOptions>(opts),
                (SendOptions opts) => RunCommand<SendCommand, SendOptions>(opts),
                errs => 1);
        }

        static int RunCommand<TCommand, TOption>(TOption options) where TOption : AbstractNatsOptions where TCommand : AbstractNatsCommand<TOption>, new()
        {
            var command = new TCommand();
            command.Init(options);
            return command.Run();
        }
    }
}
