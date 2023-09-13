using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcMonaldsSelfService.Model;

namespace WcMonaldsSelfService.ViewModel
{
    internal class MainWindowVM
    {
        public MenuItem? currentItem { get; private set; } = null;
        public List<MenuItem> items { get; private set; } = new List<MenuItem>()
        {
            new Burger("The Regular", 2.99f, new List<string>(){"Bun", "Beef Patty", "Ketchup"}),
            new Burger("The Big Max", 4.99f, new List<string>(){"Bun", "Beef Patty", "Lettuce", "Chillies", "Ketchup"}),
            new Burger("The King", 6.59f, new List<string>(){"Bun", "Beef Patty", "Bacon", "Chicken", "Ketchup", "Mayonaise", "BBQ sauce"}),
            new MenuItem("Fries", 0.99f),
            new MenuItem("Larger Fries", 1.49f),
            new MenuItem("Mt Kilofryjaro", 1.99f),
        };

        public List<MenuItem> basket { get; private set; } = new List<MenuItem>();
        public MainWindow? mw;

        public void SetItemFromList(int index, out string _name, out float _price, bool isBasket = false)
        {
            try
            {
                if (isBasket)
                {
                    currentItem = basket[index];
                }
                else
                {
                    currentItem = items[index];
                }

                _name = currentItem.Name;
                _price = currentItem.Price;
            } catch 
            {
                _name = "";
                _price = 0;
            }
        }

        public List<string> GetFullMenu()
        {
            List<string> menu = new();
            foreach (MenuItem item in items)
            {
                menu.Add(item.Name);
            }
            return menu;
        }

        public void AddItemToBasket()
        {
            try
            {
                if (currentItem != null)
                {
                    basket.Add(currentItem);
                    mw.AddItemToBasketList(currentItem.Name);
                    mw.SetTotal(GetTotalCost(basket));
                }
            } catch { }
        }

        public static float GetTotalCost(List<MenuItem> _items)
        {
            float totalCost = 0;
            foreach (MenuItem item in _items)
            {
                totalCost += item.Price;
            }
            return totalCost;
        }

        public void RemoveFromBasket()
        {
            try
            {
                basket.Remove(currentItem);
                currentItem = null;
                mw.SetTotal(GetTotalCost(basket));
                mw.RemoveItemFromBasketList();
            }
            catch { }
        }

        public void CopyAdditionalToBasket()
        {
            try
            {
                AddItemToBasket();
            }
            catch { }
        }
    }
}
