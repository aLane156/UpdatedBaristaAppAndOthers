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
                NotifyPropertyChanged(nameof(CurrentItem));
                SetPriceText(currentItem.Price);
                ShowItemSpecificMenu(currentItem);
            }
        }

        private MenuItem? selectedItemMenu;
        public MenuItem? SelectedItemMenu
        {
            get => selectedItemMenu;
            set
            {
                selectedItemMenu = value;
                NotifyPropertyChanged(nameof(SelectedItemMenu));
                if(value != null)
                {
                    SetCenterButtonStatus(true);
                    CurrentItem = selectedItemMenu;
                    SelectedItemBasket = null;
                }
            }
        }

        private MenuItem? selectedItemBasket;
        public MenuItem? SelectedItemBasket
        {
            get => selectedItemBasket;
            set
            {
                selectedItemBasket = value;
                NotifyPropertyChanged(nameof(SelectedItemBasket));
                if (value != null)
                {
                    SetCenterButtonStatus(false);
                    CurrentItem = selectedItemBasket;
                    SelectedItemMenu = null;
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

        private Visibility drinkMenuVisibility = Visibility.Collapsed;
        public Visibility DrinkMenuVisibility
        {
            get { return drinkMenuVisibility; }
            set
            {
                if (BasketButtonVisibility == Visibility.Visible)
                {
                    drinkMenuVisibility = value;
                } else
                {
                    drinkMenuVisibility = Visibility.Collapsed;
                }
                NotifyPropertyChanged(nameof(drinkMenuVisibility));
            }
        }

        private Visibility burgerMenuVisibility = Visibility.Collapsed;
        public Visibility BurgerMenuVisibility
        {
            get { return burgerMenuVisibility; }
            set
            {
                if (BasketButtonVisibility == Visibility.Visible)
                {
                    burgerMenuVisibility = value;
                }
                else
                {
                    burgerMenuVisibility = Visibility.Collapsed;
                }
                NotifyPropertyChanged(nameof(burgerMenuVisibility));
            }
        }

        private Visibility looseMeatsMenuVisibility = Visibility.Collapsed;
        public Visibility LooseMeatsMenuVisibility
        {
            get { return looseMeatsMenuVisibility; }
            set
            {
                if (BasketButtonVisibility == Visibility.Visible)
                {
                    looseMeatsMenuVisibility = value;
                    if (value == Visibility.Visible)
                    {
                        LooseMeats lm = (LooseMeats)CurrentItem;
                        CurAmount = lm.Count.ToString();
                        UpdateTotalCost();
                    }
                }
                else
                {
                    looseMeatsMenuVisibility = Visibility.Collapsed;
                }
                NotifyPropertyChanged(nameof(looseMeatsMenuVisibility));
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

        private string curAmount;
        public string CurAmount
        {
            get => curAmount;
            set
            {
                if (LooseMeatsMenuVisibility == Visibility.Visible)
                {
                    LooseMeats lm = (LooseMeats)CurrentItem;
                    int num;
                    if (int.TryParse(value, out num) && lm.ChangeNo(num))
                    {
                        curAmount = value;
                        NotifyPropertyChanged(nameof(curAmount));
                    }
                } else
                {
                    curAmount = value;
                    NotifyPropertyChanged(nameof(curAmount));
                }
            }
        }

        private string totalCost;
        public string TotalCost
        {
            get => totalCost;
            set
            {
                totalCost = value;
                NotifyPropertyChanged(nameof(totalCost));
            }
        }

        public bool MenuFocused;
        public bool BasketFocused;

        public ICommand AddToBasket { get; set; }
        public ICommand RemoveFromBasket { get; set; }
        public ICommand AddAnotherToBasket { get; set; }
        public ICommand GoToCheckout { get; set; }

        public ICommand ClickedMenu { get; set; }

        public MainWindowVM()
        {
            AddToBasket = new RelayCommand(o => AddCurrentItemToBasket(CurrentItem));
            RemoveFromBasket = new RelayCommand(o => RemoveCurrentItemFromBasket(Basket.IndexOf(CurrentItem)));
            AddAnotherToBasket = new RelayCommand(o => AddCurrentItemToBasket(CurrentItem));
            GoToCheckout = new RelayCommand(o => ToCheckout());
            ClickedMenu = new RelayCommand(o => FocusListChanged());
        }

        /// <summary>
        /// Adds an item to basket
        /// </summary>
        /// <param name="item">The item to be added</param>
        public void AddCurrentItemToBasket(MenuItem item)
        {
            try
            {
                if (currentItem != null && currentItem.GetHashCode() != nopeItem.GetHashCode())
                {
                    Basket.Add(item);
                    UpdateTotalCost();
                }
            } catch { }
        }

        /// <summary>
        /// Removes an item from basket
        /// </summary>
        /// <param name="item">The item index to remove</param>
        public void RemoveCurrentItemFromBasket(int item)
        {
            try
            {
                Basket.RemoveAt(item);
                UpdateTotalCost();
            } catch { }
        }

        /// <summary>
        /// Sets the mode of the centeral buttons to menu mode or basket mode
        /// </summary>
        /// <param name="MenuSide">Put centeral buttons into menu mode?</param>
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

        /// <summary>
        /// Sets the text showing the price of the selected item
        /// </summary>
        /// <param name="price">The raw price of the product</param>
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

        /// <summary>
        /// Shows the appropriate additional menu below the main item menu
        /// </summary>
        /// <param name="item">The current item to be checked</param>
        private void ShowItemSpecificMenu(MenuItem item)
        {
            if (item.GetType() == typeof(Drink))
            {
                DrinkMenuVisibility = Visibility.Visible;
                BurgerMenuVisibility = Visibility.Collapsed;
                LooseMeatsMenuVisibility = Visibility.Collapsed;
            } else if (item.GetType() == typeof(Burger))
            {
                DrinkMenuVisibility = Visibility.Collapsed;
                BurgerMenuVisibility = Visibility.Visible;
                LooseMeatsMenuVisibility = Visibility.Collapsed;
            }
            else if (item.GetType() == typeof(LooseMeats))
            {
                DrinkMenuVisibility = Visibility.Collapsed;
                BurgerMenuVisibility = Visibility.Collapsed;
                LooseMeatsMenuVisibility = Visibility.Visible;

            } else
            {
                DrinkMenuVisibility = Visibility.Collapsed;
                BurgerMenuVisibility = Visibility.Collapsed;
                LooseMeatsMenuVisibility = Visibility.Collapsed;
            }
        }

        private void ToCheckout()
        {

        }

        private void UpdateTotalCost()
        {
            float runningTotal = 0f;
            foreach (var item in Basket)
            {
                runningTotal += item.Price;
            }
            TotalCost = $"Total £{runningTotal}";
        }

        public void FocusListChanged()
        {

            IInputElement inputElement = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
        }
    }
}
