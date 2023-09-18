using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashierApp.Model.Types;

namespace CashierApp.Model
{
    public class DrinkProduct : Product
    {
        public DrinkProduct(string name, double price, DrinkSizes drinkSize) : base(name, price, ProductType.Drink)
        {
            DrinkSize = drinkSize;
        }

        public DrinkSizes DrinkSize { get; set; }
    }
}
