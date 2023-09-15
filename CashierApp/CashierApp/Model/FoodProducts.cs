using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public class FoodProducts
    {
        public FoodProducts() { }

        public ObservableCollection<FoodProduct> FoodProductsList { get; set; }

        /// <summary>
        /// Function for adding a whole collection to food products collection.
        /// </summary>
        /// <param name="list">The collection to be added.</param>
        public void AddToList(ObservableCollection<FoodProduct> list)
        {
            foreach (FoodProduct item in list)
            {
                FoodProductsList.Add(item);
            }
        }

        /// <summary>
        /// Function for adding a whole list to food products collection.
        /// </summary>
        /// <param name="list">The collection to be added.</param>
        public void AddToList(List<FoodProduct> list)
        {
            foreach (FoodProduct item in list)
            {
                FoodProductsList.Add(item);
            }
        }

        /// <summary>
        /// Function for clearing the class collection.
        /// </summary>
        public void ClearList()
        {
            FoodProductsList.Clear();
        }
    }
}
