using CashierApp.Model;
using CashierApp.Model.Types;
using CashierApp.Model.Backend;
using System.Collections.ObjectModel;
using System.Windows;

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

            DispatchHandler.HandleAwait(App.Current, async () => { await Database.CreateLog(); });
        }

        private void FoodItemAdded_EventHandler(Product e)
        {
            CurrentOrder.OrderItems.Add(e);
            CurrentOrder.CalculateFinal();
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
