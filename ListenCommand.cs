using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NATS.Client;
using Newtonsoft.Json.Linq;
using NLog;

namespace nats_tools
{
    public class ListenCommand : AbstractListenCommand<ListenOptions>
    {
        private new static Logger Logger { get; } = LogManager.GetCurrentClassLogger();
        public ListenCommand() : base(Logger)
        {
        }

        protected override void OnMessage(object sender, MsgHandlerEventArgs e)
        {
            NbMessages--;
            Logger logger = LogManager.GetLogger(e.Message.Subject);
            string msgTxt = Encoding.Default.GetString(e.Message.Data);
            if (Options.Json)
            {
                logger.Info($"{e.Message.Data.Length} - {JsonPrettify(msgTxt)}");
                return;
            }

            if (Options.Tokens.Any())
            {
                    var logTxt = $"{e.Message.Data.Length} - ";
                    logTxt += string.Join(Options.Delimiter, ExtractTokens(msgTxt));
                    logger.Info(logTxt);
            }
            else
            {
                logger.Info($"{e.Message.Data.Length} - '{msgTxt}'");
            }
        }

        protected IEnumerable<string> ExtractTokens(string txt)
        {
            try
            {
                JObject o = JObject.Parse(txt);
                return Options.Tokens.Select(token => o.SelectToken(token).ToString());
            }
            catch (Exception ex)
            {
                Logger.Error($"{ex.Message}, JSon message: {txt}");
                return null;
            }
        }
    }
}