using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcMonaldsSelfService.Model;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace WcMonaldsSelfService.ViewModel
{
    internal class MainWindowVM : BaseVM
    {
        private readonly MenuItem nopeItem = new MenuItem("Select an item", 0f);

        private MenuItem? currentItem;
        public MenuItem CurrentItem
        {
            get => currentItem;
            set
            {
                currentItem = value;
                NotifyPropertyChanged(nameof(currentItem));
                SetPriceText(currentItem.Price);
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
                    CurrentItem = menu[(int)_selectedIndex];
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
                    catch 
                    {
                        if (value > 0)
                        {
                            CurrentItem = Basket.Last<MenuItem>();
                        } else
                        {
                            CurrentItem = nopeItem;
                        }

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

        private string curPrice;
        public string CurPrice
        {
            get => curPrice;
            set
            {
                curPrice = value;
                NotifyPropertyChanged(nameof(curPrice));
            }
        }

        public ICommand AddToBasket { get; set; }
        public ICommand RemoveFromBasket { get; set; }
        public ICommand AddAnotherToBasket { get; set; }

        public MainWindowVM()
        {
            AddToBasket = new RelayCommand(o => AddCurrentItemToBasket(CurrentItem));
            RemoveFromBasket = new RelayCommand(o => RemoveCurrentItemFromBasket(Basket.IndexOf(CurrentItem)));
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

        public void RemoveCurrentItemFromBasket(int item)
        {
            try
            {
                Basket.RemoveAt(item);
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

        private void SetPriceText(float price)
        {
            if (price == 0)
            {
                CurPrice = string.Empty;
            } else if (price > 0)
            {
                CurPrice = "£" + price.ToString();
            } 
        }
    }
}
