using System;
using System.IO;
using System.Linq;
using System.Text;
using NATS.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace nats_tools
{
    internal class ListenCommand : AbstractNatsCommand<ListenOptions>
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        public override int Run()
        {
            if (Options.Subjects == null || !Options.Subjects.Any())
            {
                Options.Subjects = new[] { ">" };
            }
            foreach (var subject in Options.Subjects)
            {
                logger.Info($"Listen: {subject}");
                Options.Connection.SubscribeAsync(subject, OnMessage);
            }

            return 0;
        }

        private void OnMessage(object sender, MsgHandlerEventArgs e)
        {
            Logger logger = LogManager.GetLogger(e.Message.Subject);
            string msgTxt = Encoding.Default.GetString(e.Message.Data);
            if (Options.Json)
            {
                logger.Info($"{e.Message.Data.Length} - {JsonPrettify(msgTxt)}");
                return;
            }

            if (Options.Tokens.Any())
            {
                JObject o = JObject.Parse(msgTxt);
                var logTxt = $"{e.Message.Data.Length} - ";
                logTxt += string.Join(Options.Delimiter, Options.Tokens.Select(token => o.SelectToken(token).ToString()));
                logger.Info(logTxt);
                return;
            }

            logger.Info($"{e.Message.Data.Length} - '{msgTxt}'");
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