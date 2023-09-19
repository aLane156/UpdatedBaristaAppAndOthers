using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsingCompositeCommands.Models
{
    public class BurgerModel : Item
    {
        private List<string> lItems = new List<string>();

        public List<string> LItems 
        { 
            get { return lItems; } 
            private set { lItems = value; }
        }
    }

    //public BurgerModel(string _name, float _price, List<string> lItems): base(_name, _price)
    //{
    //    LItems = lItems;
    //}
}
