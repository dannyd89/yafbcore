using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlattiBase.Helper
{
    public static class Log
    {
        //public static Dictionary<string, TimeSpan> Logs = new Dictionary<string, TimeSpan>();
        public static List<LogEntry> Logs = new List<LogEntry>();

        public static void AddLogEntry(string entryName, TimeSpan elapsed)
        {
            //TimeSpan temp;
            //if (Logs.TryGetValue(entryName, out temp))
            //{
            //    if (elapsed > temp)
            //        Logs[entryName] = elapsed;
            //}
            //else
            //    Logs.Add(entryName, elapsed);
            Logs.Add(new LogEntry(entryName, elapsed));
        }

        public static void ExportLogs()
        {
            Logs.Sort();

            StringBuilder sb = new StringBuilder();

            foreach (LogEntry logEntry in Logs)
                sb.AppendLine(logEntry.Name + ": " + logEntry.Elapsed.ToString());

            File.WriteAllText("log-" + DateTime.Now.ToString("MM-dd-yyyy HH-mm-ss-fff") + ".txt", sb.ToString());
        }
        
    }
}
