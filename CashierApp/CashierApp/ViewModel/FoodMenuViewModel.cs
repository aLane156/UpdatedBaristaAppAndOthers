using CashierApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierApp.ViewModel
{
	internal class FoodMenuViewModel : BaseViewModel
	{
		public FoodMenuViewModel() 
		{
			FoodMenuItems = new ObservableCollection<FoodProduct>();

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

			DispatchHandler.HandleAwait(App.Current, async () => await Database.CreateJson());
			//DispatchHandler.HandleAwait(App.Current, async () => await Database.WriteFile(tempFoodList));
			// TODO: Test this + get value.
			DispatchHandler.HandleAwait(App.Current, async () => await Database.ReadJson(ProductType.Food));
		}

		public RelayCommand AddItemCommand { get; set; }

		public RelayCommand DeselectCommand { get; set; }

		private ObservableCollection<FoodProduct> _foodMenu;

		private FoodProduct? _foodSelected;

		public FoodProduct? FoodSelected 
		{ 
			get => _foodSelected;
			set
			{
				SetProperty(ref _foodSelected, value);
			}
		}

		public ObservableCollection<FoodProduct> FoodMenuItems
		{
			get => _foodMenu;
			set => SetProperty(ref _foodMenu, value);
		}

		private void DeselectFunction(object obj)
		{
			FoodSelected = null;
		}

		private void AddItem(object obj)
		{
			if (FoodSelected != null)
			{
				SelectedEventAggregator.BroadCast(FoodSelected);
			}
		}

		private bool CanDeselect(object obj)
		{
			if (FoodSelected != null)
			{
				return true;
			}
			return false;
		}

		private bool CanAdd(object obj)
		{
			if (FoodSelected == null)
			{
				return false;
			}
			return true;
		}
	}
}
