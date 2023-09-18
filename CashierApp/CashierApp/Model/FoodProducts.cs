using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashierApp.Model.Types;

namespace CashierApp.Model
{
    public class FoodProduct : Product
    {
        /// <summary>
        /// The class for food products.
        /// </summary>
        /// <param name="name">Name of the food product.</param>
        /// <param name="price">Price of the food product.</param>
        /// <param name="expirationDate">Expiration date of the food product. Uses DateTime but is date only.</param>
        public FoodProduct(string name, double price) : base(name, price, ProductType.Food)
        {
            ExpirationDate = DateTime.Now;
        }

        private DateTime _expirationDate;

        // adding another property for demo purposes
        public DateTime ExpirationDate { get => _expirationDate; set => _expirationDate = value; }
    }

    public struct FoodProducts
    {
        public ObservableCollection<FoodProduct> FoodProductsList { get; set; }
    }
}
