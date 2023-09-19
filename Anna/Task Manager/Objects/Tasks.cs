using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Manager
{
    public class Task
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
    }

    public class TaskList : Dictionary<string, Task>
    {

    }
}
