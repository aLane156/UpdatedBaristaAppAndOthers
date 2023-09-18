using CafeTillApp.Models;
using System.Collections.Generic;
using Prism.Mvvm;
using Prism.Commands;
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
        private Dictionary<string, Dictionary<string, double>> _currentDictionary;
        public Dictionary<string, Dictionary<string, double>> CurrentDictionary
        {
            get { return _currentDictionary; }
            set { SetProperty(ref _currentDictionary, value); }
        }

        private readonly IEventAggregator _eventAggregator;

        public MenuViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            Inventory inventory = new Inventory();

            CurrentDictionary = new Dictionary<string, Dictionary<string, double>>();
            CurrentDictionary = inventory.dict;
        }

        /// <summary>
        /// Carries option on and pushes to next subMenuView
        /// </summary>
        /// <param name="option"></param>
        void ExecuteButtonCommand(string option)
        {
            MainWindowViewModel.SharedOption.Option = option;

            var newView = new SubMenuView();
            _eventAggregator.GetEvent<ChangeViewEvent>().Publish(newView);
        }
    }
}
