using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierApp.Model
{
    public class Order
    {
        private static double TaxPercentage = 0.2;

        public Order()
        {
            Id = Guid.NewGuid().ToString();
            OrderItems = new ObservableCollection<Product>();
        }

        public string Id { get; set; }

        private ObservableCollection<Product> _orderItems;

        public ObservableCollection<Product> OrderItems 
        { 
            get => _orderItems;
            set
            {
                _orderItems = value;
                CalculateFinal();
            }
        }

        private double beforeTaxTotal;

        public double TaxAmount { get; private set; }

        public double FinalPrice { get; set; }

        private void CalculateTotal()
        {
            foreach (var item in OrderItems)
            {
                beforeTaxTotal += item.Price;
            }
        }

        private void CalculateTax()
        {
            CalculateTotal();
            TaxAmount = Math.Truncate(100 * (beforeTaxTotal * TaxPercentage)) / 100;
        }

        public void CalculateFinal()
        {
            CalculateTax();
            FinalPrice = Math.Truncate(100 * (beforeTaxTotal + TaxAmount)) / 100;
        }

        public void Clear()
        {
            Id = string.Empty;
            OrderItems.Clear();
            beforeTaxTotal = 0;
            TaxAmount = 0;
            FinalPrice = 0;
        }
    }
}
