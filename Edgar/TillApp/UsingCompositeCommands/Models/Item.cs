using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace UsingCompositeCommands.Models
{
    public class Item
    {
        private string name = "";
        private float price;

        public string Name 
        { 
            get { return name; }
            private set { name = value; }
        }

        public float Price
        {
            get { return price; }
            protected set { price = value; }
        }
        public Item()
        {
            Name = string.Empty;
            Price = 0;
        }
        public Item(string _name, float _price)
        {
            Name = _name;
            Price = _price;
 
        }
    }
}
