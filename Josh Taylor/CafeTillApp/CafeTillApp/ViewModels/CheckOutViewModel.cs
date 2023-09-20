using CafeTillApp.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CafeTillApp.ViewModels
{
    public class CheckOutViewModel : BindableBase
    {
        public ICommand BackCommand { get; private set; }
        public ICommand PayCommand { get; private set; }
        public ICommand EnterKeyCommand { get; private set; }

        private string _tips = null;
        public string Tips
        {
            get { return _tips; }
            set
            {
                _tips = value;
                OnPropertyChanged("Tips");
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
        
        /// <summary>
        /// On pay wipe basket and go back to start
        /// Complete reset
        /// </summary>
        private void PayCommandExecute()
        {
            MainWindowViewModel.SharedBasket.Basket.Clear();

            var newView = new MenuView();
            _eventAggregator.GetEvent<ChangeViewEvent>().Publish(newView);
        }
        /// <summary>
        /// when enter pressed when textbox focused
        /// adds tips to basket and wipes texbox by reseting UI
        /// all only happens if texbox holding a float
        /// </summary>
        public void EnterKeyPressed()
        {
            if (float.TryParse(Tips, out _))
            {
                if (MainWindowViewModel.SharedBasket.Basket == null)
                {
                    MainWindowViewModel.SharedBasket.Basket = new ObservableCollection<string>();
                }

                decimal tipValue;
                if (Decimal.TryParse(Tips, out tipValue))
                {
                    string formattedTips = tipValue.ToString("F2");
                    MainWindowViewModel.SharedBasket.Basket.Add("Tip \n£" + formattedTips);
                }

                var newView = new CheckOutView();
                _eventAggregator.GetEvent<ChangeViewEvent>().Publish(newView);
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
