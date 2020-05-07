using System;
using System.Collections.Generic;
using System.Threading;
using NATS.Client;
using NLog;

namespace nats_tools
{
    public class StatCommand : AbstractListenCommand<StatOptions>
    {
        private new static Logger Logger { get; } = LogManager.GetCurrentClassLogger();
        private Thread logThread;
        private bool stopRequest;
        public  DateTime StartDate { get; }
        private readonly Dictionary<string, MessageStat> statBySubject = new Dictionary<string, MessageStat>();  

        public StatCommand() : base(Logger)
        {
            StartDate = DateTime.Now;
            logThread = new Thread(Start)
            {
                Name = "Nats Stat Log"
            };
            logThread.Start();
        }

        private void Start()
        {
            var period = TimeSpan.FromMilliseconds(Options.Period);
            DateTime nextTime = DateTime.Now + period;
            while (!stopRequest)
            {
                var now = DateTime.Now;
                if (now > nextTime)
                {
                    nextTime = now + period;
                    var totalTimeInS = (now - StartDate).TotalSeconds;
                    Logger.Info("-----------------------------------------------------------------------------------------------------");
                    Logger.Info(MessageStat.Header);
                    Logger.Info("-----------------------------------------------------------------------------------------------------");
                    foreach (var stat in statBySubject.Values)
                    {
                        Logger.Info(stat.ToString(totalTimeInS));
                        stat.Reset();
                    }
                }
                
                Thread.Sleep(100);
            }
        }

        protected override void OnMessage(object sender, MsgHandlerEventArgs e)
        {
            var subject = e.Message.Subject;
            lock (statBySubject)
            {
                if (!statBySubject.TryGetValue(subject, out var stat))
                {
                    stat = new MessageStat(subject);
                    statBySubject[subject] = stat;
                }

                stat.Update(e.Message.Data.Length);            }
        }

        protected override void Dispose()
        {
            stopRequest = true;
        }
    }

    internal class MessageStat
    {
        public static string Header { get; } = " Count         Size   DeltaC       DeltaS   Count/s       Size/s   Subject";
        public string Subject { get; }
        public int Count { get; set; }
        public long Size { get; set; }
        public int PrevCount { get; set; }
        public long PrevSize { get; set; }

        public MessageStat(string subject)
        {
            Subject = subject;
        }

        public void Update(in int size)
        {
            Size += size;
            Count++;
        }

        public void Reset()
        {
            PrevCount = Count;
            PrevSize = Size;
        }

        public string ToString(double totalTimeInS)
        {
            return $"{Count,6:###,##0} {Size,12:###,###,##0}   {Count-PrevCount,6:###,##0} {Size-PrevSize,12:###,###,##0} {Count/totalTimeInS,9:###,##0.00} {Size/totalTimeInS,12:###,###,##0.00}   {Subject}";
        }
    }
}