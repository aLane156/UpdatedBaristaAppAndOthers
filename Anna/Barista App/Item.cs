using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barista_App
{
    public class Item
    {
        public string Name;
        public string ItemID;
        public string Price;
    }

    public class OrderItem : Item
    {

    }

    public class Order: Dictionary<object, Item>
    {

    }

    public class Menu: Dictionary<object, Item>
    {

    }
}
