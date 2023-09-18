using CashierApp.Model;
using CashierApp.Model.Backend;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace CashierApp.ViewModel
{
    internal class DrinkMenuViewModel : BaseViewModel
    {
        public DrinkMenuViewModel()
        {
            MenuItems = new();

            ObservableCollection<DrinkProduct> tempDrinkList = new()
            {
                new("Coke", 5.30),
                new("Pepsi", 6.00),
                new("Sparkling Water", 8.35),
                new("Chocolate Milkshake", 4.25),
                new("Hot Chocolate", 7.20),
                new("Latte", 5.00)
            };

            DispatchHandler.HandleAwait(App.Current, async () =>
            {
                var data = await Database.ReadJsonDrink();
                foreach (DrinkProduct product in data)
                {
                    MenuItems.Add(product);
                }
            });

            AddItemCommand = new RelayCommand(AddItem, CanAdd);
            DeselectCommand = new RelayCommand(DeselectFunction, CanDeselect);
        }

        public RelayCommand AddItemCommand { get; set; }

        public RelayCommand DeselectCommand { get; set; }

        private DrinkProduct? _selected;

        public DrinkProduct? Selected
        {
            get => _selected;
            set { SetProperty(ref _selected, value); }
        }

        private ObservableCollection<DrinkProduct> _menuItems;

        public ObservableCollection<DrinkProduct> MenuItems
        {
            get { return _menuItems; }
            set
            {
                _menuItems = value;
                NotifyPropertyChanged(nameof(MenuItems));
            }
        }

        private void DeselectFunction(object obj)
        {
            Selected = null;
        }

        private void AddItem(object obj)
        {
            if (Selected != null)
            {
                SelectedDrinkEventAggregator.BroadCast(Selected);
            }
        }

        private bool CanDeselect(object obj)
        {
            if (Selected != null)
            {
                return true;
            }
            return false;
        }

        private bool CanAdd(object obj)
        {
            if (Selected != null)
            {
                return true;
            }
            return false;
        }
    }
}
