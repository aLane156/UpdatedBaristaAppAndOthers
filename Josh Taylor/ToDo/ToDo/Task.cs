using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo
{
    public class Task
    {
        public string description { get; set; }
        public bool complete { get; set; }
        public string date { get; set; }
        public string tag { get; set; }
    }
    public class TaskList : Dictionary<string, Task>
    {
    }
    public class TaskSelected
    {
        public int id = -1;
        public string description = null;
        public bool complete = false;
        public int date = -1;
        public string tag = "";
    }
}
