using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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
            CompleteList = new TodoItems();
            var Program = new Database();

            Program.CreateJson();

            DispatcherTimer Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += new EventHandler(Timer_Tick);

            Timer.Start();
            UpdateGrid();

            SubmitTodo = new RelayCommand(AddTodo, CanSubmit);
            SubmitArchive = new RelayCommand(DelTodo, CanDelete);
            SubmitClear = new RelayCommand((object obj) => GridSelected = null, CanDeselect);
            
            DeadlineDate = DateTime.Now;

            // registering property changed event
            ReRegisterEvents();
        }

        private void GridChangedHandler(object? sender, PropertyChangedEventArgs e)
        {
            // TODO: fix being called every character.
            var DatabaseProgram = new Database();

            DatabaseProgram.WriteJson(CompleteList);
            //Debug.WriteLine(e.PropertyName);
        }

        public event EventHandler GridChanged;

        public ICommand SubmitTodo { get; set; }

        public ICommand SubmitArchive { get; set; }

        public ICommand SubmitClear { get; set; }

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
            }
        }

        public TodoItems CompleteList { get; set; }

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

        public DateTime DeadlineDate
        {
            get => _deadlineDate;
            set
            {
                _deadlineDate = value;
                NotifyPropertyChanged(nameof(DeadlineDate));
            }
        }

        private DateTime _deadlineDate;

        private TodoItem _gridSelected;

        private ObservableCollection<TodoItem> _gridItems;

        private void AddTodo(object obj)
        {
            var DatabaseProgram = new Database();

            TodoItem TempTodo = new TodoItem(InputTitle, InputDesc, DateOnly.FromDateTime(DeadlineDate));

            GridItems.Add(TempTodo);
            CompleteList.Todos.Add(TempTodo);

            DatabaseProgram.WriteJson(CompleteList);

            InputTitle = "";
            InputDesc = "";
            DeadlineDate = DateTime.Now;

            ReRegisterEvents();
        }

        private void DelTodo(object obj)
        {
            var DatabaseProgram = new Database();
            TodoItem temp = GridSelected;

            TodoItem GridFound = GridItems.FirstOrDefault(x => x.Id == temp.Id);
            TodoItem ListFound = CompleteList.Todos.FirstOrDefault(x => x.Id == temp.Id);

            GridFound.Archived = true;
            ListFound.Archived = true;

            DatabaseProgram.WriteJson(CompleteList);
            UpdateGrid();

            ReRegisterEvents();
        }

        private void UpdateGrid()
        {
            var DatabaseProgram = new Database();
            TodoItems FetchedItems = DatabaseProgram.ReadJson();

            GridItems.Clear();
            CompleteList.Todos.Clear();

            if (FetchedItems != null)
            {
                foreach (TodoItem item in FetchedItems.Todos)
                {
                    if (item.Archived == true)
                    {
                        CompleteList.Todos.Add(item);
                        continue;
                    }
                    GridItems.Add(item);
                    CompleteList.Todos.Add(item);
                }
            }

            ReRegisterEvents();
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

        //private void WriteJsonWrapper(ObservableCollection<TodoItem> JsonItems)
        //{
        //    Database DatabaseProgram = new Database();
        //    TodoItems TodoProgram = new TodoItems();

        //    try
        //    {
        //        DatabaseProgram.WriteJson(TodoProgram.ConvertTodoList(JsonItems));
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine($"[TodoViewModel] caught writing failure: {e.Message}");
        //    }
        //    finally
        //    {

        //    }
        //}

        private void ReRegisterEvents()
        {
            foreach (TodoItem item in CompleteList.Todos)
            {
                if (item != null)
                {
                    item.PropertyChanged += GridChangedHandler;
                }
            }
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
