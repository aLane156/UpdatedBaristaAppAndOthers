using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using WPFApp.Model;

namespace WPFApp.ViewModel
{
    public class TodoViewModel : BaseViewModel
    {
        public TodoViewModel()
        {
            GridItems = new ObservableCollection<TodoItem>();
            var Program = new Database();

            Program.CreateJson();

            DispatcherTimer Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += new EventHandler(Timer_Tick);

            Timer.Start();
            UpdateGrid();

            SubmitTodo = new RelayCommand(AddTodo, CanSubmit);
            SubmitDelete = new RelayCommand(DelTodo, CanDelete);
            SubmitClear = new RelayCommand((object obj) => GridSelected = null, CanDeselect);
            SubmitComplete = new RelayCommand(CompleteTodo);
            DeadlineDate = DateOnly.FromDateTime(DateTime.Now);
        }

        public ICommand SubmitTodo { get; set; }

        public ICommand SubmitDelete { get; set; }

        public ICommand SubmitClear { get; set; }

        public ICommand SubmitComplete { get; set; }

        public TodoItem? GridSelected
        {
            get => _gridSelected;
            set
            {
                _gridSelected = value;
                NotifyPropertyChanged(nameof(GridSelected));
            }
        }

        public ObservableCollection<TodoItem> GridItems
        {
            get => _gridItems;
            set
            {
                _gridItems = value;
                NotifyPropertyChanged(nameof(GridItems));
            }
        }

        private string _title, _description, _clockFace;

        public string ClockFace
        {
            get => _clockFace;
            set
            {
                _clockFace = value;
                NotifyPropertyChanged(nameof(ClockFace));
            }
        }

        public string InputTitle
        {
            get => _title;
            set
            {
                _title = value;
                NotifyPropertyChanged(nameof(InputTitle));
            }
        }

        public string InputDesc
        {
            get => _description;
            set
            {
                _description = value;
                NotifyPropertyChanged(nameof(InputDesc));
            }
        }

        public DateOnly DeadlineDate
        {
            get => _deadlineDate;
            set
            {
                _deadlineDate = value;
                NotifyPropertyChanged(nameof(DeadlineDate));
            }
        }

        private DateOnly _deadlineDate;

        private TodoItem _gridSelected;

        private ObservableCollection<TodoItem> _gridItems;

        private void AddTodo(object obj)
        {
            var DatabaseProgram = new Database();
            var TodoProgram = new TodoItems();

            GridItems.Add(new TodoItem(InputTitle, InputDesc, DeadlineDate));

            DatabaseProgram.WriteJson(TodoProgram.ConvertTodoList(GridItems));

            InputTitle = "";
            InputDesc = "";
            DeadlineDate = DateOnly.FromDateTime(DateTime.Now);
        }

        private void DelTodo(object obj)
        {
            var DatabaseProgram = new Database();
            var TodoProgram = new TodoItems();

            GridItems.Remove(GridSelected);
            DatabaseProgram.WriteJson(TodoProgram.ConvertTodoList(GridItems));
        }

        private void CompleteTodo(object obj)
        {
            var DatabaseProgram = new Database();
            var TodoProgram = new TodoItems();

            GridSelected.Completed = !GridSelected.Completed;
            DatabaseProgram.WriteJson(TodoProgram.ConvertTodoList(GridItems));

            GridItems.Clear();
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            var DatabaseProgram = new Database();

            TodoItems FetchedItems = DatabaseProgram.ReadJson();

            if (FetchedItems != null)
            {
                foreach (TodoItem item in FetchedItems.Todos)
                {
                    GridItems.Add(item);
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime Time = DateTime.Now;
            var SecondsProgram = new Seconds(Time.Second);
            var MinutesProgram = new Minutes(Time.Minute);
            var HoursProgram = new Hours(Time.Hour);

            ClockFace = $"{HoursProgram.Output}:{MinutesProgram.Output}:{SecondsProgram.Output} - {Time.Day}/{Time.Month}/{Time.Year}";
        }

        private bool CanSubmit(object obj)
        {
            if (string.IsNullOrEmpty(InputTitle) || string.IsNullOrEmpty(InputDesc))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CanDelete(object obj)
        {
            if (GridSelected != null)
            {
                return true;
            }
            return false;
        }

        private bool CanDeselect(object obj)
        {
            if (GridSelected != null)
            {
                return true;
            }
            return false;
        }
    }

    public class Seconds
    {
        public Seconds(int Seconds)
        {
            if (Seconds < 10)
            {
                this.Output = $"0{Seconds}";
            }
            else
            {
                this.Output = Seconds.ToString();
            }
        }

        public string Output { get; set; }
    }

    public class Minutes
    {
        public Minutes(int Minutes)
        {
            if (Minutes < 10)
            {
                this.Output = $"0{Minutes}";
            }
            else
            {
                this.Output = Minutes.ToString();
            }
        }

        public string Output { get; set; }
    }

    public class Hours
    {
        public Hours(int Hours)
        {
            if (Hours < 10)
            {
                this.Output = $"0{Hours}";
            }
            else
            {
                this.Output = Hours.ToString();
            }
        }

        public string Output { get; set; }
    }
}
