using CommandLine;

namespace nats_tools
{
    public static class Program
    {
        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<ListenOptions, SendOptions, RequestOptions, ReplyOptions>(args)
              .MapResult(
                (ListenOptions opts) => RunCommand<ListenCommand, ListenOptions>(opts),
                (RequestOptions opts) => RunCommand<RequestCommand, RequestOptions>(opts),
                (SendOptions opts) => RunCommand<SendCommand, SendOptions>(opts),
                (ReplyOptions opts) => RunCommand<ReplyCommand, ReplyOptions>(opts),
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
