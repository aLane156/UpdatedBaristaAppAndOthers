using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashierApp.Model.Types;

namespace CashierApp.Model.Backend
{
    public class LogEntry
    {
        /// <summary>
        /// The class for storing the log file entries.
        /// </summary>
        /// <param name="logText">The main body of the log.</param>
        /// <param name="logFile">The file from where the log was activated.</param>
        /// <param name="logType">The type of log, use LogType enum.</param>
        public LogEntry(string logText, string logFile, LogType logType)
        {
            LogDate = DateTime.Now;
            LogText = logText;
            LogFile = logFile;
            LogType = logType;
        }

        public DateTime LogDate { get; set; }

        public string LogFile { get; set; }

        public LogType LogType { get; set; }

        public string LogText { get; set; }

        public override string ToString()
        {
            return $"{LogDate} - {LogFile} - {LogType} - {LogText}";
        }
    }
}
