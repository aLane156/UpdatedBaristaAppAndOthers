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
			AddItemCommand = new RelayCommand(AddItem, CanAdd);
			DeselectCommand = new RelayCommand(DeselectFunction, CanDeselect);

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
				SelectedFoodEventAggregator.BroadCast(Selected);
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
