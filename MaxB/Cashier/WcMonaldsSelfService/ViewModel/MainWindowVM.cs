using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcMonaldsSelfService.Model;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;

namespace WcMonaldsSelfService.ViewModel
{
    internal class MainWindowVM : BaseVM
    {
        private MenuItem? currentItem;
        public MenuItem CurrentItem
        {
            get => currentItem;
            set
            {
                currentItem = value;
                NotifyPropertyChanged(nameof(currentItem));
            }
        }

        private int? _selectedIndex;
        public int? selectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                NotifyPropertyChanged(nameof(_selectedIndex));
                if (value != null) 
                {
                    SetCenterButtonStatus(true);
                    selectedIndexBasket = null;
                    try
                    {
                        CurrentItem = menu[(int)_selectedIndex];
                    } catch { }
                    finally
                    {
                        CurrentItem = menu[0];
                    }
                }
            }
        }

        private int? _selectedIndexBasket;
        public int? selectedIndexBasket
        {
            get => _selectedIndexBasket;
            set
            {
                _selectedIndexBasket = value;
                NotifyPropertyChanged(nameof(_selectedIndexBasket));
                if (value != null)
                {
                    SetCenterButtonStatus(false);
                    selectedIndex = null;
                    try
                    {
                        CurrentItem = Basket[(int)_selectedIndexBasket];
                    }
                    catch { }
                    finally
                    {
                        CurrentItem = Basket[Basket.Count - 1];
                    }
                }
            }
        }

        private List<MenuItem> menu = new()
        {
            new Burger("The Regular", 2.99f, new List<string>(){"Bun", "Beef Patty", "Ketchup"}),
            new Burger("The Big Max", 4.99f, new List<string>(){"Bun", "Beef Patty", "Lettuce", "Chillies", "Ketchup"}),
            new Burger("The King", 6.59f, new List<string>(){"Bun", "Beef Patty", "Bacon", "Chicken", "Ketchup", "Mayonaise", "BBQ sauce"}),
            new MenuItem("Fries", 0.99f),
            new MenuItem("Larger Fries", 1.49f),
            new MenuItem("Mt Kilofryjaro", 1.99f),
            new LooseMeats("Small Nuggets", 1.39f, 12),
            new LooseMeats("Large Nuggets", 2.99f, 25),
            new LooseMeats("Garlic Bread Slices", 1.49f, 6),
            new Drink("Bottled Water", 0.99f, new DrinkSize[] {DrinkSize.small, DrinkSize.medium, DrinkSize.large}),
            new Drink("Pepsi Max", 1.50f, new DrinkSize[] {DrinkSize.small, DrinkSize.medium }),
            new Drink("Oasis", 1.99f, new DrinkSize[] { DrinkSize.small}),
        };
        public List<MenuItem> Menu
        {
            get => menu;
            set
            {
                menu = value;
                NotifyPropertyChanged(nameof(Menu));
            }
        }

        private ObservableCollection<MenuItem> basket = new();
        public ObservableCollection<MenuItem> Basket
        {
            get => basket;
            set
            {
                basket = value;
                NotifyPropertyChanged(nameof(basket));
            }
        }

        private Visibility centerButtonVisibility = Visibility.Visible;
        public Visibility CenterButtonVisibility
        {
            get { return centerButtonVisibility; }
            set 
            { 
                centerButtonVisibility = value;
                NotifyPropertyChanged(nameof(centerButtonVisibility));
            }
        }

        private Visibility basketButtonVisibility = Visibility.Collapsed;
        public Visibility BasketButtonVisibility
        {
            get { return basketButtonVisibility; }
            set
            {
                basketButtonVisibility = value;
                NotifyPropertyChanged(nameof(basketButtonVisibility));
            }
        }

        public ICommand AddToBasket { get; set; }
        public ICommand RemoveFromBasket { get; set; }
        public ICommand AddAnotherToBasket { get; set; }

        public MainWindowVM()
        {
            AddToBasket = new RelayCommand(o => AddCurrentItemToBasket(CurrentItem));
            RemoveFromBasket = new RelayCommand(o => RemoveCurrentItemFromBasket(CurrentItem));
            AddAnotherToBasket = new RelayCommand(o => AddCurrentItemToBasket(CurrentItem));
        }

        public void AddCurrentItemToBasket(MenuItem item)
        {
            try
            {
                if (currentItem != null)
                {
                    Basket.Add(item);
                }
            } catch { }
        }

        public void RemoveCurrentItemFromBasket(MenuItem item)
        {
            try
            {
                Basket.Remove(item);
            } catch { }
        }

        private void SetCenterButtonStatus(bool MenuSide)
        {
            if (MenuSide)
            {
                CenterButtonVisibility = Visibility.Visible;
                BasketButtonVisibility = Visibility.Collapsed;
            }
            else
            {
                CenterButtonVisibility = Visibility.Collapsed;
                BasketButtonVisibility = Visibility.Visible;
            }
        }
    }
}
