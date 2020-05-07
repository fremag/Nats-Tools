using CommandLine;

namespace nats_tools
{
    public static class Program
    {
        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments(args, typeof(ListenOptions), typeof(SendOptions), typeof(RequestOptions), typeof(ReplyOptions), typeof(StatOptions))
              .MapResult(
                (ListenOptions opts) => RunCommand<ListenCommand, ListenOptions>(opts),
                (RequestOptions opts) => RunCommand<RequestCommand, RequestOptions>(opts),
                (SendOptions opts) => RunCommand<SendCommand, SendOptions>(opts),
                (ReplyOptions opts) => RunCommand<ReplyCommand, ReplyOptions>(opts),
                (StatOptions opts) => RunCommand<StatCommand, StatOptions>(opts),
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
