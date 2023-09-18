using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CafeTillApp.Views;

namespace CafeTillApp.ViewModels
{
    public class BasketViewModel : INotifyPropertyChanged
    {
        private string _totalText = "Total: £0";
        private string _buttonText = "Checkout";
        private ICommand _checkOutCommand;
        private ICommand _removeItemCommand;

        // checking if the view next to it is ckecked out
        private bool _isCheckOutView;
        public bool IsCheckOutView
        {
            get { return _isCheckOutView; }
            set
            {
                _isCheckOutView = value;
                OnPropertyChanged("IsCheckOutView");
            }
        }

        private readonly IEventAggregator _eventAggregator;
        public BasketViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            // When the view changes 
            _eventAggregator.GetEvent<ChangeViewEvent>().Subscribe(OnViewChanged);

            // Check if MainWindowViewModel.SharedBasket.Basket is not null
            if (MainWindowViewModel.SharedBasket.Basket == null)
            {
                // Initialize MainWindowViewModel.SharedBasket.Basket as an empty ObservableCollection<string>
                MainWindowViewModel.SharedBasket.Basket = new ObservableCollection<string>();
            }

            // Subscribe to the CollectionChanged event
            MainWindowViewModel.SharedBasket.Basket.CollectionChanged += BasketItems_CollectionChanged;
        }

        private void OnViewChanged(object view)
        {
            // Update IsCheckOutView based on the current view
            IsCheckOutView = view is CheckOutView;
        }

        /// <summary>
        /// Binding basket items
        /// </summary>
        public ObservableCollection<string> BasketItems
        {
            get { return MainWindowViewModel.SharedBasket.Basket; }
        }

        /// <summary>
        /// Binding total cost
        /// </summary>
        public string TotalText
        {
            get { return _totalText; }
            private set
            {
                _totalText = value;
                OnPropertyChanged("TotalText");
            }
        }

        /// <summary>
        /// Binding button text
        /// </summary>
        public string ButtonText
        {
            get { return _buttonText; }
            set
            {
                _buttonText = value;
                OnPropertyChanged("ButtonText");
            }
        }

        /// <summary>
        /// Binding checkout button press
        /// </summary>
        public ICommand CheckOutCommand
        {
            get
            {
                if (_checkOutCommand == null)
                {
                    _checkOutCommand = new RelayCommand(CheckOut);
                }
                return _checkOutCommand;
            }
        }

        /// <summary>
        /// sending left view to chekout screen
        /// </summary>
        private void CheckOut()
        {
            var newView = new CheckOutView();
            IsCheckOutView = newView is CheckOutView;
            _eventAggregator.GetEvent<ChangeViewEvent>().Publish(newView);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Removing item command binding 
        /// </summary>
        public ICommand RemoveItemCommand
        {
            get
            {
                if (_removeItemCommand == null)
                {
                    _removeItemCommand = new RelayCommand<string>(RemoveItem);
                }
                return _removeItemCommand;
            }
        }

        private void RemoveItem(string item)
        {
            MainWindowViewModel.SharedBasket.Basket.Remove(item);
        }

        private void BasketItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Recalculate TotalText whenever the collection changes
            TotalText = CalculateTotal();
        }

        private string CalculateTotal()
        {
            // Check if BasketItems is not null
            if (BasketItems != null)
            {
                // Calculate the total
                float total = 0;
                foreach (string item in BasketItems)
                {
                    // Find the index of '£'
                    int index = item.IndexOf('£');
                    if (index >= 0)
                    {
                        // Remove everything before and including '£'
                        string substring = item.Substring(index + 1);
                        // Try to parse the remaining string to an int
                        if (float.TryParse(substring, out float value))
                        {
                            // Add the value to the total
                            total += value;
                        }
                    }
                }
                // Return the total as a string
                return "Total: £"+total.ToString();
            }
            else
            {
                // If BasketItems is null, return an empty string
                return "";
            }
        }

    }
}
