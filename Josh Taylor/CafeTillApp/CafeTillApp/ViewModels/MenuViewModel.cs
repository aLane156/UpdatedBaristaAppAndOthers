using CafeTillApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Commands;
using System.Windows.Controls;
using System.Windows.Input;
using Prism.Events;
using CafeTillApp.Views;

namespace CafeTillApp.ViewModels
{
    public class MenuViewModel : BindableBase
    {
        // Example on how to set up data binding
        /*
        private string _content = "test button";
        public string Content
        {
            get { return _content; }
            set { SetProperty(ref _content, value); }
        }
        */
        private DelegateCommand<string> _buttonCommand;
        public DelegateCommand<string> ButtonCommand =>
            _buttonCommand ?? (_buttonCommand = new DelegateCommand<string>(ExecuteButtonCommand));

        // Binding the current dictionary to the buttons they relate to
        private Dictionary<string, string[]> _currentDictionary;
        public Dictionary<string, string[]> CurrentDictionary
        {
            get { return _currentDictionary; }
            set { SetProperty(ref _currentDictionary, value); }
        }

        public MenuViewModel()
        {
            Inventory inventory = new Inventory();

            CurrentDictionary = new Dictionary<string, string[]>();
            CurrentDictionary = inventory.dict;
        }

        private readonly IEventAggregator _eventAggregator;

        public MenuViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void ChangeView()
        {
            var newView = new SubMenuView();
            _eventAggregator.GetEvent<ChangeViewEvent>().Publish(newView);
        }

        void ExecuteButtonCommand(string parameter)
        {
            Console.WriteLine($"Button clicked: {parameter}");
            ChangeView();
        }
    }
}
