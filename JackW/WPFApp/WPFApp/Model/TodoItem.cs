using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WPFApp.Model
{
    public class TodoItems
    {
        public TodoItems()
        {
            Todos = new List<TodoItem>();
        }

        public List<TodoItem> Todos { get; set; }

        public TodoItems ConvertTodoList(ObservableCollection<TodoItem> obCollection)
        {
            TodoItems TempTodoItems = new TodoItems();
            
            foreach (TodoItem item in obCollection) 
            {
                TempTodoItems.Todos.Add(item);
            }

            return TempTodoItems;
        }
    }

    public class TodoItem
    {
        [JsonConstructor]
        public TodoItem(string Title, string Desc, DateOnly Deadline)
        {
            var Program = new Database();

            Id = Database.GenUUID();
            this.Title = Title;
            Description = Desc;
            Date = DateOnly.FromDateTime(DateTime.Now);
            this.Deadline = Deadline;
            Completed = false;
            Archived = false;
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly Date { get; set; }
        public DateOnly Deadline { get; set; }
        public bool Completed { get; set; }
        public bool Archived { get; set; }

        public override string ToString()
        {
            return $"{this.Title}, {this.Description}, {this.Date}, {this.Deadline}";
        }
    }
}
