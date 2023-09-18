using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashierApp.Model.Types;

namespace CashierApp.Model
{
    public class DrinkProduct : Product
    {
        public DrinkProduct(string name, double price) : base(name, price, ProductType.Drink)
        {
            DrinkSize = DrinkSizes.Small;
        }

        public DrinkSizes DrinkSize { get; set; }
    }

    public struct DrinkProducts
    {
        public ObservableCollection<DrinkProduct> DrinkProductsList { get; set; }
    }
}
