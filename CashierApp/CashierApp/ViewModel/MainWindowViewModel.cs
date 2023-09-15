using CashierApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace CashierApp.ViewModel
{
    internal class MainWindowViewModel : BaseViewModel
    {
        private FoodMenuViewModel _foodMenuViewModel = new FoodMenuViewModel();

        public MainWindowViewModel()
        {
            CurrentViewModel = _foodMenuViewModel;
            CurrentOrder = new Order();
            ProductsSelected = new ObservableCollection<Product>();

            MinimiseApp = new RelayCommand((e) => App.Current.MainWindow.WindowState = WindowState.Minimized);
            CloseApp = new RelayCommand((e) => App.Current.Shutdown());

            SelectedEventAggregator.OnMessageTransmitted += FoodItemAdded_EventHandler;

            

            App.Current.Dispatcher.Invoke(() => { Database.CreateLog(); });
        }

        private void FoodItemAdded_EventHandler(Product e)
        {
            CurrentOrder.OrderItems.Add(e);
        }

        public RelayCommand MinimiseApp { get; set; }

        public RelayCommand CloseApp { get; set; }

        private ObservableCollection<Product> _prodList, _prodSelected;

        private BaseViewModel _currentView;

        public ObservableCollection<Product> ProductsSource
        {
            get => _prodList;
            set
            {
                _prodList = value;
            }
        }

        public ObservableCollection<Product> ProductsSelected { get => _prodSelected; set => _prodSelected = value; }

        public BaseViewModel CurrentViewModel
        {
            get => _currentView;
            set { SetProperty(ref _currentView, value); }
        }

        private Order _currentOrder;

        public Order CurrentOrder 
        { 
            get => _currentOrder;
            set => SetProperty(ref _currentOrder, value);
        }
    }
}
