using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace CashierApp.Model
{
    public abstract class Product
    {
        /// <summary>
        /// Base class for all products.
        /// </summary>
        /// <param name="name">The name of the product.</param>
        /// <param name="desc">The products description.</param>
        /// <param name="price">The price of the product.</param>
        /// <param name="productType">The type of product. ProductType enum.</param>
        public Product(string name, double price, ProductType productType) 
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Description = string.Empty;
            Price = price;
            Type = productType;
        }

        public string Id { get => _id; set => _id = value; }

        public string Name { get => _name; set => _name = value; }

        public string Description { get =>  _desc; set => _desc = value; }

        public double Price { get => _price; set => _price = value; }

        public ProductType Type { get; set; }

        private string _id, _name, _desc;

        private double _price;

        public override string ToString()
        {
            return $"{Id}:{Name}:{Description}:{Price}";
        }
    }
}
