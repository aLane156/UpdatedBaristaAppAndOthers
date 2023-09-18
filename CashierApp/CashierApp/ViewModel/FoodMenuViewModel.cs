using CashierApp.Model.Backend;
using CashierApp.Model.Types;
using CashierApp.Model;
using System.Collections.ObjectModel;

namespace CashierApp.ViewModel
{
    internal class FoodMenuViewModel : BaseViewModel
	{
		public FoodMenuViewModel()
		{
			MenuItems = new ObservableCollection<FoodProduct>();
			ObservableCollection<FoodProduct> temp = new();

			ObservableCollection<FoodProduct> tempFoodList = new()
			{
				new("Pizza", 13.52),
				new("Pasta", 9.00),
				new("Lasagna", 15.50),
				new("Risotto", 8.00),
				new("Gnocchi", 11.30),
				new("Spaghetti Bolognese", 12.50)
			};

			AddItemCommand = new RelayCommand(AddItem, CanAdd);
			DeselectCommand = new RelayCommand(DeselectFunction, CanDeselect);
			//LoadCommand = new RelayCommand(async o => 
			//{
   //             var data = await Database.ReadJson(ProductType.Food);

			//	foreach (var item in data)
			//	{
			//		// HAS TO BE .Add otherwise it messes up references
			//		FoodMenuItems.Add(item);
			//	}
   //         });

			DispatchHandler.HandleAwait(App.Current, async () => await Database.CreateJson());
			DispatchHandler.HandleAwait(App.Current, async () => 
			{ 
				var data = await Database.ReadJsonFood(); 
				foreach (var item in data)
				{
					MenuItems.Add(item);
				}
			});
		}

		public RelayCommand Refresh { get; set; }

		public RelayCommand LoadCommand { get; set; }

		public RelayCommand AddItemCommand { get; set; }

		public RelayCommand DeselectCommand { get; set; }

		private ObservableCollection<FoodProduct> _menuItems;

		private FoodProduct? _menuSelected;

		public FoodProduct? Selected 
		{ 
			get => _menuSelected;
			set
			{
				SetProperty(ref _menuSelected, value);
			}
		}

		public ObservableCollection<FoodProduct> MenuItems
		{
			get => _menuItems;
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
				SelectedEventAggregator.BroadCast(Selected);
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
