using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFApp.Model;

namespace WPFApp.ViewModel
{
    public class ArchiveViewModel : BaseViewModel
    {
        public ArchiveViewModel() 
        {
            CompleteList = new TodoItems();
            ArchiveGridItems = new ObservableCollection<TodoItem>();

            UpdateArchive();

            SubmitDelete = new RelayCommand(DeleteTodo, CanDelete);
            SubmitRestore = new RelayCommand(RestoreArchive, CanDelete);
        }

        public TodoItems CompleteList { get; set; }

        public TodoItem ArchiveSelected
        {
            get => _archSelected;
            set
            {
                _archSelected = value;
                NotifyPropertyChanged(nameof(ArchiveSelected));
            }
        }

        private TodoItem _archSelected;

        public ICommand SubmitDelete { get; set; }

        public ICommand SubmitRestore { get; set; }

        public ObservableCollection<TodoItem> ArchiveGridItems
        {
            get => _archItems;
            set
            {
                _archItems = value;
            }
        }

        private ObservableCollection<TodoItem> _archItems;

        private void UpdateArchive()
        {
            var DatabaseProgram = new Database();
            TodoItems ItemArray = DatabaseProgram.ReadJson();

            ArchiveGridItems.Clear();

            if (ItemArray != null)
            {
                foreach (TodoItem item in ItemArray.Todos)
                {
                    if (item.Archived == false) 
                    { 
                        CompleteList.Todos.Add(item);
                        continue; 
                    }
                    ArchiveGridItems.Add(item);
                    CompleteList.Todos.Add(item);
                }
            }
        }

        private void DeleteTodo(object obj)
        {
            var DatabaseProgram = new Database();
            TodoItem TempTodo = ArchiveSelected;

            ArchiveGridItems.Remove(TempTodo);
            CompleteList.Todos.Remove(TempTodo);

            DatabaseProgram.WriteJson(CompleteList);
        }

        private void RestoreArchive(object obj)
        {
            var DatabaseProgram = new Database();
            TodoItem TempTodo = ArchiveSelected;

            TodoItem ArchFound = ArchiveGridItems.FirstOrDefault(x => x.Id == TempTodo.Id);
            TodoItem ListFound = CompleteList.Todos.FirstOrDefault(x => x.Id == TempTodo.Id);

            ArchFound.Archived = false;
            ListFound.Archived = false;

            DatabaseProgram.WriteJson(CompleteList);
            UpdateArchive();
        }

        private bool CanDelete(object obj)
        {
            if (ArchiveSelected != null)
            {
                return true;
            }
            return false;
        }
    }
}
