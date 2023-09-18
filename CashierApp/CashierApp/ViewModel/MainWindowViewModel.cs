using CashierApp.Model;
using CashierApp.Model.Types;
using CashierApp.Model.Backend;
using System.Collections.ObjectModel;
using System.Windows;
using System;

namespace CashierApp.ViewModel
{
    internal class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel()
        {
            FoodMenuVM = new();
            DrinkMenuVM = new();

            CurrentViewModel = FoodMenuVM;
            CurrentOrder = new Order();
            ProductsSelected = new ObservableCollection<Product>();

            MinimiseApp = new RelayCommand((e) => App.Current.MainWindow.WindowState = WindowState.Minimized);
            CloseApp = new RelayCommand((e) => App.Current.Shutdown());

            FoodViewCommand = new RelayCommand(o => { CurrentViewModel = FoodMenuVM; });
            DrinkViewCommand = new RelayCommand(o => { CurrentViewModel = DrinkMenuVM; });

            RemoveOrderItem = new RelayCommand(RemoveOrderItemFunction, CanRemoveOrderItem);

            SelectedFoodEventAggregator.OnMessageTransmitted += FoodItemAdded_EventHandler;
            SelectedDrinkEventAggregator.OnMessageTransmitted += DrinkItemAdded_EventHandler;

            DispatchHandler.HandleAwait(App.Current, async () => await Database.CreateLog());
            DispatchHandler.HandleAwait(App.Current, async () => await Database.CreateJson());
        }

        private void FoodItemAdded_EventHandler(Product e)
        {
            CurrentOrder.OrderItems.Add(e);
            CurrentOrder.CalculateFinal();
        }

        private void DrinkItemAdded_EventHandler(Product e)
        {
            CurrentOrder.OrderItems.Add(e);
            CurrentOrder.CalculateFinal();
        }

        public RelayCommand RemoveOrderItem { get; set; }

        public RelayCommand DrinkViewCommand { get; set; }
        public RelayCommand FoodViewCommand { get; set; }

        public FoodMenuViewModel FoodMenuVM { get; set; }
        public DrinkMenuViewModel DrinkMenuVM { get; set; }

        public RelayCommand MinimiseApp { get; set; }
        public RelayCommand CloseApp { get; set; }

        private Product _orderSelection;

        public Product? OrderSelection
        {
            get { return _orderSelection; }
            set
            {
                _orderSelection = value;
                NotifyPropertyChanged(nameof(OrderSelection));
            }
        }

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
            set
            {
                _currentView = value;
                NotifyPropertyChanged(nameof(CurrentViewModel));
            }
        }

        private Order _currentOrder;

        public Order CurrentOrder 
        { 
            get => _currentOrder;
            set => SetProperty(ref _currentOrder, value);
        }

        private void RemoveOrderItemFunction(object obj)
        {
            Product copy = OrderSelection;

            if (copy != null)
            {
                CurrentOrder.OrderItems.Remove(copy);
                CurrentOrder.CalculateFinal();
            }

            OrderSelection = null;
        }

        private bool CanRemoveOrderItem(object obj)
        {
            if (OrderSelection != null)
            {
                return true;
            }
            else return false;
        }
    }
}
