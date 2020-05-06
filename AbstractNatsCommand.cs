using System.IO;
using Newtonsoft.Json;
using NLog;

namespace nats_tools
{
    public abstract class AbstractNatsCommand<T> where T : AbstractNatsOptions
    {
        protected Logger Logger { get; }
        protected T Options {get; private set;}
        
        public abstract int Run();

        protected AbstractNatsCommand(Logger logger)
        {
            Logger = logger;
        }

        internal void Init(T options)
        {
            Options = options;
            Options.Start();
        }
        
        public static string JsonPrettify(string json)
        {
            using (var stringReader = new StringReader(json))
            using (var stringWriter = new StringWriter())
            {
                var jsonReader = new JsonTextReader(stringReader);
                var jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented };
                jsonWriter.WriteToken(jsonReader);
                return stringWriter.ToString();
            }
        }
    }
}