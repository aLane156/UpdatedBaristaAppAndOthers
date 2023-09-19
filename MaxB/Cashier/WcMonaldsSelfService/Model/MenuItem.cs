using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcMonaldsSelfService.Model
{
    public class MenuItem
    {
        private string name = "";
        private float price;

        public string Name
        {
            get { return name; }
            private set
            {
                if (value == null) { return; }
                name = value;
            }
        }

        public float Price
        {
            get { return price; }
            protected set
            {
                if (value >= 0)
                {
                    price = value;
                }
            }
        }

        public MenuItem()
        {
            Name = "";
            Price = 0;
        }

        public MenuItem(string _name, float _price)
        {
            Name = _name;
            Price = _price;
        }

        public override string ToString()
        {
            return $"{name}";
        }

        public MenuItem Clone()
        {
            MenuItem menuItem = new();
            menuItem.Name = Name;
            menuItem.Price = Price;
            return menuItem;
        }
    }
}
