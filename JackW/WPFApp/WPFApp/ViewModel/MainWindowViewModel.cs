using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Automation;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using WPFApp.Model;

namespace WPFApp.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel() 
        { 
            TodoVM = new TodoViewModel();
            ArchiveVM = new ArchiveViewModel();

            CurrentViewModel = TodoVM;

            TodoCommand = new RelayCommand(o => { CurrentViewModel = TodoVM; });
            ArchiveCommand = new RelayCommand(o => {  CurrentViewModel = ArchiveVM; });
        }

        public ICommand TodoCommand { get; set; }
        public ICommand ArchiveCommand { get; set; }

        private BaseViewModel _currentViewModel;

        public BaseViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                NotifyPropertyChanged(nameof(CurrentViewModel));
            }
        }

        private static TodoViewModel TodoVM { get; set; }
        private static ArchiveViewModel ArchiveVM { get; set; }
    }
}
