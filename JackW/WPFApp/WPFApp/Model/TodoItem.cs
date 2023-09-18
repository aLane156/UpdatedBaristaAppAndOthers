using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

    public class TodoItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public TodoItem(string Title, string Desc, DateOnly Deadline)
        {
            var Program = new Database();

            Id = Program.GenUUID();
            this.Title = Title;
            Description = Desc;
            Date = DateOnly.FromDateTime(DateTime.Now);
            this.Deadline = Deadline;
            Completed = false;
            Archived = false;
        }

        public string Id
        {
            get => _id;
            set => _id = value;
        }
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("title"));
            }
        }
        public string Description 
        { 
            get => _desc; 
            set
            {
                _desc = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("description"));
            }
        }
        public DateOnly Date
        {
            get => _date;
            set => _date = value;
        }
        public DateOnly Deadline
        {
            get => _deadline;
            set
            {
                _deadline = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("deadline"));
            }
        }
        public int DaysRemaining { get; set; }
        public bool Completed
        {
            get => _comp;
            set
            {
                _comp = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("completed"));
            }
        }
        public bool Archived
        {
            get => _arch;
            set => _arch = value;
        }

        private string _id, _title, _desc;

        private DateOnly _date, _deadline;

        private bool _comp, _arch;

        public void CalcRemainingDays()
        {
            DateOnly CurrentDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly TempDeadline = this.Deadline;

            this.DaysRemaining = TempDeadline.DayNumber - CurrentDate.DayNumber;
        }

        public override string ToString()
        {
            return $"{this.Title}, {this.Description}, {this.Date}, {this.Deadline}";
        }
    }
}
