using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApp.Model
{
    public class ConsoleOutput
    {
        public ConsoleOutput(string Source, string Message) 
        {
            TimeStamp = TimeOnly.FromDateTime(DateTime.Now).ToString("HH:mm:ss");

            this.Source = $"[{Source}]";
            this.Message = Message;
        }

        public string Source { get; set; }
        public string TimeStamp { get; set; }
        public string Message { get; set; }
    }
}
