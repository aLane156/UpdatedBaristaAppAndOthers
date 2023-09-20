using System;
using System.Collections.Generic;
using WcMonaldsSelfService.Model;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;

namespace WcMonaldsSelfService.ViewModel
{
    internal class MainWindowVM : BaseVM
    {
        private readonly MenuItem nopeItem = new("Select an item", 0f);

        private MenuItem? currentItem;
        public MenuItem? CurrentItem
        {
            get => currentItem;
            set
            {
                currentItem = value;
                NotifyPropertyChanged(nameof(CurrentItem));
                if (currentItem != null)
                {
                    SetPriceText(currentItem.Price);
                    ShowItemSpecificMenu(currentItem);
                } else
                {
                    SetPriceText(0);
                    ShowItemSpecificMenu();
                }
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
                    if (selectedItemBasket is Drink drink)
                    {
                        DrinkSizes = drink.AcceptedSizes;
                    }
                }
            }
        }

        private List<MenuItem> menu = new()
        {
            new Burger("The Regular", 1.99f, new List<string>(){"Bun", "Beef Patty", "Ketchup"}),
            new Burger("The Big Max", 2.99f, new List<string>(){"Bun", "Beef Patty", "Lettuce", "Chillies", "Ketchup"}),
            new Burger("The King", 4.59f, new List<string>(){"Bun", "Beef Patty", "Bacon", "Chicken", "Ketchup", "Mayonaise", "BBQ sauce"}),
            new MenuItem("Small Fries", 0.99f),
            new MenuItem("Fries", 1.49f),
            new MenuItem("Large Fries", 1.99f),
            new LooseMeats("Chicken Nuggets", 1.39f, 12),
            new LooseMeats("Chicken Wings", 2.99f, 25),
            new LooseMeats("Garlic Bread Slices", 1.49f, 6),
            new Drink("Bottled Water", 0.99f, new DrinkSize[] {DrinkSize.small, DrinkSize.medium, DrinkSize.large}),
            new Drink("Pepsi Max", 1.49f, new DrinkSize[] {DrinkSize.small, DrinkSize.medium }),
            new Drink("Sprite", 1.99f, new DrinkSize[] { DrinkSize.small}),
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

        private List<Deal> Deals = new();

        private ObservableCollection<MenuItem> basket = new();
        public ObservableCollection<MenuItem> Basket
        {
            get => basket;
            set
            {
                basket = value;
                NotifyPropertyChanged(nameof(Basket));
            }
        }

        private Visibility centerButtonVisibility = Visibility.Visible;
        public Visibility CenterButtonVisibility
        {
            get { return centerButtonVisibility; }
            set
            {
                centerButtonVisibility = value;
                NotifyPropertyChanged(nameof(CenterButtonVisibility));
            }
        }

        private Visibility basketButtonVisibility = Visibility.Collapsed;
        public Visibility BasketButtonVisibility
        {
            get { return basketButtonVisibility; }
            set
            {
                basketButtonVisibility = value;
                NotifyPropertyChanged(nameof(BasketButtonVisibility));
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
                NotifyPropertyChanged(nameof(DrinkMenuVisibility));
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
                NotifyPropertyChanged(nameof(BurgerMenuVisibility));
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
                NotifyPropertyChanged(nameof(LooseMeatsMenuVisibility));
            }
        }

        private string curPrice;
        public string CurPrice
        {
            get => curPrice;
            set
            {
                curPrice = value;
                NotifyPropertyChanged(nameof(CurPrice));
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
                    LooseMeats? lm = CurrentItem as LooseMeats;
                    if (lm == null) { return; }
                    if (int.TryParse(value, out int num) && lm.ChangeNo(num))
                    {
                        curAmount = value;
                        NotifyPropertyChanged(nameof(CurAmount));
                        UpdateTotalCost();
                        CurPrice = lm.Price.ToString();
                    }
                } else
                {
                    curAmount = value;
                    NotifyPropertyChanged(nameof(CurAmount));
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
                NotifyPropertyChanged(nameof(TotalCost));
            }
        }

        private int currentTab;
        public int CurrentTab
        {
            get => currentTab;
            set
            {
                currentTab = value;
                NotifyPropertyChanged(nameof(CurrentTab));
            }
        }

        private DrinkSize[] drinkSizes = {DrinkSize.small};
        public DrinkSize[] DrinkSizes
        {
            get => drinkSizes;
            set
            {
                drinkSizes = value;
                NotifyPropertyChanged(nameof(DrinkSizes));
            }
        }

        private DrinkSize curDrinkSize;
        public DrinkSize CurDrinkSize
        {
            get => curDrinkSize;
            set
            {
                curDrinkSize = value;
                NotifyPropertyChanged(nameof(CurDrinkSize));
                ((Drink)CurrentItem).SetSize(curDrinkSize);
                UpdateTotalCost();
                CurPrice = CurrentItem.Price.ToString();
            }
        }

        private float totalDiscount;
        public float TotalDiscount
        {
            get => totalDiscount;
            set
            {
                totalDiscount = value;
                NotifyPropertyChanged(nameof(TotalDiscount));
            }
        }

        private Visibility payVis = Visibility.Visible;
        public Visibility PayVis
        {
            get { return payVis; }
            set
            {
                payVis = value;
                NotifyPropertyChanged(nameof(PayVis));
            }
        }

        private Visibility payedVis = Visibility.Collapsed;
        public Visibility PayedVis
        {
            get { return payedVis; }
            set
            {
                payedVis = value;
                NotifyPropertyChanged(nameof(PayedVis));
            }
        }

        public bool MenuFocused;
        public bool BasketFocused;

        private string payedText;
        public string PayedText
        {
            get { return payedText; }
            set
            {
                payedText = value;
                NotifyPropertyChanged(nameof(PayedText));
            }
        }

        public string debugLines;
        public string DebugLines
        {
            get { return debugLines; }
            set
            {
                debugLines = value;
                NotifyPropertyChanged(nameof(DebugLines));
            }
        }

        public ICommand AddToBasket { get; set; }
        public ICommand RemoveFromBasket { get; set; }
        public ICommand AddAnotherToBasket { get; set; }
        public ICommand GoToCheckout { get; set; }
        public ICommand GoToMenu { get; set; }
        public ICommand NextCustomer { get; set; }
        public ICommand SwaptoPayedScreen { get; set; }

        public MainWindowVM()
        {
            AddToBasket = new RelayCommand(o => SelectAddCurrentItemToBasket(CurrentItem));
            RemoveFromBasket = new RelayCommand(o => RemoveCurrentItemFromBasket(Basket.IndexOf(CurrentItem)));
            AddAnotherToBasket = new RelayCommand(o => AddCurrentItemToBasket(CurrentItem));
            GoToCheckout = new RelayCommand(o => CurrentTab = 1);
            GoToMenu = new RelayCommand(o => CurrentTab = 0);
            NextCustomer = new RelayCommand(o => ResetForNextCustomer());
            SwaptoPayedScreen = new RelayCommand(o => SetCheckoutVisabilities(true));

            Deals.Add(new Deal(new MenuItem[] { Menu[1], Menu[4] }, 1));
        }

        private void SelectAddCurrentItemToBasket(MenuItem item)
        {
            if (item == null) return;
            if (item is LooseMeats)
            {
                AddCurrentItemToBasket(item as LooseMeats);
            } else if (item is Burger)
            {
                AddCurrentItemToBasket(item as Burger);
            }
            else if (item is Drink)
            {
                AddCurrentItemToBasket(item as Drink);
            }
            else
            {
                AddCurrentItemToBasket(item);
            }
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
                    MenuItem menuItem = item.Clone();

                    Basket.Add(menuItem);
                    UpdateTotalCost();
                }
            } catch (Exception ex)
            {
                AddErrorToErrorOutput(ex);
            }
        }

        /// <summary>
        /// Adds a LooseMeats to basket
        /// </summary>
        /// <param name="item">The item to be added</param>
        public void AddCurrentItemToBasket(LooseMeats item)
        {
            try
            {
                if (currentItem != null && currentItem.GetHashCode() != nopeItem.GetHashCode())
                {
                    LooseMeats menuItem = new(item.Name, item.Price, item.Count);

                    Basket.Add(menuItem);
                    UpdateTotalCost();
                }
            }
            catch (Exception ex)
            {
                AddErrorToErrorOutput(ex);
            }
        }

        public void AddCurrentItemToBasket(Burger item)
        {
            try
            {
                if (currentItem != null && currentItem.GetHashCode() != nopeItem.GetHashCode())
                {
                    Burger menuItem = new(item.Name, item.Price, item.Layers);

                    Basket.Add(menuItem);
                    UpdateTotalCost();
                }
            }
            catch (Exception ex)
            {
                AddErrorToErrorOutput(ex);
            }
        }

        public void AddCurrentItemToBasket(Drink item)
        {
            try
            {
                if (currentItem != null && currentItem.GetHashCode() != nopeItem.GetHashCode())
                {
                    Drink menuItem = new(item.Name, item.Price, item.AcceptedSizes);

                    Basket.Add(menuItem);
                    UpdateTotalCost();
                }
            }
            catch (Exception ex)
            {
                AddErrorToErrorOutput(ex);
            }
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
            } catch (Exception ex) 
            {
                AddErrorToErrorOutput(ex);
            }
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
        /// Hides all menus
        /// </summary>
        private void ShowItemSpecificMenu()
        {
            DrinkMenuVisibility = Visibility.Collapsed;
            BurgerMenuVisibility = Visibility.Collapsed;
            LooseMeatsMenuVisibility = Visibility.Collapsed;
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

        /// <summary>
        /// Updates the total cost by counting items in the basket
        /// </summary>
        private void UpdateTotalCost()
        {
            CheckDeals();
            float runningTotal = 0f;
            foreach (var item in Basket)
            {
                runningTotal += item.Price;
            }
            runningTotal -= TotalDiscount;
            runningTotal = MathF.Round(runningTotal * 100);
            TotalCost = $"Total £{runningTotal / 100}";
        }

        /// <summary>
        /// Resets all appropriate fields, making it as if the application was just opened.
        /// </summary>
        private void ResetForNextCustomer()
        {
            Basket.Clear();
            UpdateTotalCost();
            CurrentTab = 0;
            SelectedItemBasket = null;
            SelectedItemMenu = null;
            CurrentItem = null;
            SetCheckoutVisabilities(false);
        }

        private void SetCheckoutVisabilities(bool showPayed)
        {
            if (showPayed)
            {
                PayedVis = Visibility.Visible;
                PayVis = Visibility.Collapsed;
                int seconds = DateTime.Now.Second;
                PayedText = $"Your order number is {MathF.Floor(seconds * MathF.PI)}, enjoy your meal!";
            }
            else
            {
                PayVis = Visibility.Visible;
                PayedVis = Visibility.Collapsed;
                PayedText = string.Empty;
            }
        }


        private void CheckDeals()
        {
            float discount;
            MenuItem[] basketArray = new MenuItem[Basket.Count];
            for (int i = 0; i < Basket.Count; i++) { basketArray[i] = Basket[i]; }

            foreach (var Deal in Deals)
            {
                if (Deal.CheckIfApplies(basketArray, out discount))
                {
                    TotalDiscount += discount;
                }
            }
        }
        /// <summary>
        /// Adds an error to the debug error list
        /// </summary>
        /// <param name="e"></param>
        private void AddErrorToErrorOutput(Exception e)
        {
            DebugLines += $"ERROR: {e.Message} at: \n {e.StackTrace} \n";
        }
    }
}
