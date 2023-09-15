using CafeTillApp.Views;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Mvvm;
using System.Windows.Controls;
using Microsoft.VisualBasic.FileIO;
using CafeTillApp.Models;
using System.Collections.ObjectModel;

namespace CafeTillApp.ViewModels
{
    internal class SubMenuViewModel : BindableBase
    {
        // Button comands
        public ICommand BackCommand { get; private set; }

        private DelegateCommand<string> _buttonCommand;
        public DelegateCommand<string> ButtonCommand =>
            _buttonCommand ?? (_buttonCommand = new DelegateCommand<string>(ExecuteButtonCommand));

        private readonly IEventAggregator _eventAggregator;
        private string option; // Declare the field without initializing it here

        private Dictionary<string, Dictionary<string, double>> _currentDictionary;
        public Dictionary<string, Dictionary<string, double>> CurrentDictionary
        {
            get { return _currentDictionary; }
            set { SetProperty(ref _currentDictionary, value); }
        }

        public SubMenuViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            BackCommand = new DelegateCommand(BackCommandExecute);
            option = MainWindowViewModel.SharedOption.Option; // Initialize the field in the constructor

            Inventory inventory = new Inventory();

            CurrentDictionary = new Dictionary<string, Dictionary<string, double>>();
            CurrentDictionary = inventory.dict;
        }

        /// <summary>
        /// Binding selectedkey
        /// </summary>
        public string SelectedKey
        {
            get { return option; }
            set
            {
                option = value;
            }
        }

        /// <summary>
        /// Creates selected items dictionary
        /// </summary>
        public Dictionary<string, double> SelectedItems
        {
            get
            {
                if (CurrentDictionary != null && SelectedKey != null && CurrentDictionary.ContainsKey(SelectedKey))
                {
                    return CurrentDictionary[SelectedKey];
                }
                else
                {
                    return new Dictionary<string, double>();
                }
            }
        }
        /// <summary>
        /// Sets up list to be displayed and combinds the naem and the value
        /// </summary>
        public List<string> CombinedItems
        {
            get
            {
                if (SelectedItems != null)
                {
                    List<string> combinedItems = new List<string>();
                    foreach (KeyValuePair<string, double> item in SelectedItems)
                    {
                        combinedItems.Add(string.Format("{0}\n£{1}", item.Key, item.Value));
                    }
                    return combinedItems;
                }
                else
                {
                    return new List<string>();
                }
            }
        }
        /// <summary>
        /// Changes view back to Menu
        /// </summary>
        private void BackCommandExecute()
        {
            var newView = new MenuView();
            _eventAggregator.GetEvent<ChangeViewEvent>().Publish(newView);
        }

        void ExecuteButtonCommand(string item)
        {
            if (MainWindowViewModel.SharedBasket.Basket == null)
            {
                MainWindowViewModel.SharedBasket.Basket = new ObservableCollection<string>();
            }

            MainWindowViewModel.SharedBasket.Basket.Add(item);
        }

    }
}