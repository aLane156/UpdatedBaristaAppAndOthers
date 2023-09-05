using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using WPFApp.Model;
using WPFApp.View;

namespace WPFApp.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel() 
        { 
            TodoVM = new TodoViewModel();
            ArchiveVM = new ArchiveViewModel();

            CurrentViewModel = TodoVM;
            //ConsoleWindow window = new ConsoleWindow();
            ConsoleStatus = true;

            Output = new List<ConsoleOutput>();
            AddOutput("MainWindowViewModel", "Console View Running.");

            TodoCommand = new RelayCommand(o => CurrentViewModel = TodoVM);
            ArchiveCommand = new RelayCommand(o => CurrentViewModel = ArchiveVM);

            MinimiseApp = new RelayCommand(o => App.Current.MainWindow.WindowState = WindowState.Minimized);
            CloseApp = new RelayCommand(o => App.Current.Shutdown());

            OpenConsole = new RelayCommand(o => {/* window.Show();*/ ConsoleStatus = false; });

            //this.ConsoleMessage += ConsoleWindow.HandleMessageEvent;
        }

        public event EventHandler ConsoleMessage;

        public ICommand TodoCommand { get; set; }
        public ICommand ArchiveCommand { get; set; }

        public ICommand MinimiseApp { get; set; }
        public ICommand CloseApp { get; set; }

        public ICommand OpenConsole { get; set; }

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

        private void AddOutput(string Source, string Message)
        {
            Output.Add(new ConsoleOutput(Source, Message));
        }

        public List<ConsoleOutput> Output { get; set; }

        public bool ConsoleStatus
        {
            get => _conStat;
            set
            {
                _conStat = value;
                NotifyPropertyChanged(nameof(ConsoleStatus));
            }
        }
        
        private bool _conStat;
    }
}
