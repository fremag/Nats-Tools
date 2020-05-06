namespace nats_tools
{
    internal abstract class AbstractNatsCommand<T> where T : AbstractNatsOptions
    {
        protected T Options {get; private set;}
        public abstract int Run();

        internal void Init(T options)
        {
            Options = options;
            Options.Start();
        }
    }
}