using CafeTillApp.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace CafeTillApp.ViewModels
{
    class CheckOutViewModel : BindableBase
    {
        public ICommand BackCommand { get; private set; }
        public ICommand PayCommand { get; private set; }
        public ICommand EnterKeyCommand { get; private set; }

        private string _tips = "";
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
            EnterKeyCommand = new RelayCommand<object>(EnterKeyPressed);
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
            if (float.TryParse(Tips, out _))
            {
                if (MainWindowViewModel.SharedBasket.Basket == null)
                {
                    MainWindowViewModel.SharedBasket.Basket = new ObservableCollection<string>();
                }

                MainWindowViewModel.SharedBasket.Basket.Add("£"+Tips);
                // Clear the property
                Tips = "";
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
