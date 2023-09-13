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
        private MenuItem currentItem = new MenuItem();
        public List<MenuItem> Items { get; set; } = new List<MenuItem>()
        {
            new Burger("The Big Max", 5.99f, new List<string>(){"Bun", "Beef Patty", "Lettuce", "Jalapenos", "Ketchup"})
        };

        public MenuItem CurrentItem
        {
            get { return currentItem; }
            set { if (value != null) { currentItem = value; } }
        }

        public void SetItemFromList(int index)
        {
            currentItem = Items[index];
        }

        public void SetItemFromList(int index, out string _name, out float _price)
        {
            currentItem = Items[index];
            _name = currentItem.Name;
            _price = currentItem.Price;
        }

        public List<string> GetFullMenu()
        {
            List<string> menu = new();
            foreach (MenuItem item in Items)
            {
                menu.Add(item.Name);
            }
            return menu;
        }

        public void AddItemToBasket()
        {
            if (currentItem != null)
            {

            }
        }
    }
}
