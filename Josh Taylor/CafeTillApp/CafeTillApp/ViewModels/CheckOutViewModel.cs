using CafeTillApp.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CafeTillApp.ViewModels
{
    class CheckOutViewModel : BindableBase
    {
        public ICommand BackCommand { get; private set; }
        public ICommand PayCommand { get; private set; }
        public ICommand EnterKeyCommand { get; private set; }

        private string _yourProperty = "";
        public string YourProperty
        {
            get { return _yourProperty; }
            set
            {
                _yourProperty = value;
                OnPropertyChanged("YourProperty");
            }
        }
        public new event PropertyChangedEventHandler PropertyChanged;

        private readonly IEventAggregator _eventAggregator;
        public CheckOutViewModel(IEventAggregator eventAggregator)
        {
            BackCommand = new DelegateCommand(BackCommandExecute);
            PayCommand = new DelegateCommand(PayCommandExecute);
            EnterKeyCommand = new RelayCommand(EnterKeyPressed);
            _eventAggregator = eventAggregator;
        }

        /// <summary>
        /// Changes view back to Menu
        /// </summary>
        private void BackCommandExecute()
        {
            var newView = new MenuView();
            _eventAggregator.GetEvent<ChangeViewEvent>().Publish(newView);
        }
        
        private void PayCommandExecute()
        {
            MainWindowViewModel.SharedBasket.Basket.Clear();
            var newView = new MenuView();
            _eventAggregator.GetEvent<ChangeViewEvent>().Publish(newView);
        }
        private void EnterKeyPressed(object obj)
        {
            // Clear the property
            YourProperty = string.Empty;
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
