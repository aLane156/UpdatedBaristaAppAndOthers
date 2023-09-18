using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CashierApp.Model.Types;

namespace CashierApp.Model
{
    public class Order : INotifyPropertyChanged
    {
        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

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

        private double beforeTaxTotal, _taxAmount, _finalPrice;

        public double TaxAmount
        {
            get { return _taxAmount; }
            private set
            {
                _taxAmount = value;
                NotifyPropertyChanged(nameof(TaxAmount));
            }
        }

        public double FinalPrice
        {
            get { return _finalPrice; }
            private set
            {
                _finalPrice = value;
                NotifyPropertyChanged(nameof(FinalPrice));
            }
        }

        public string DisplayFinal
        {
            get { return $"£{FinalPrice.ToString("#.##")}"; }
        }

        public string DisplayTax
        {
            get { return $"£{TaxAmount.ToString("#.##")}"; }
        }

        #region Methods

        private void CalculateTotal()
        {
            beforeTaxTotal = 0;

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
            NotifyPropertyChanged(nameof(DisplayTax));
            NotifyPropertyChanged(nameof(DisplayFinal));
        }

        public void Clear()
        {
            Id = string.Empty;
            OrderItems.Clear();
            beforeTaxTotal = 0;
            TaxAmount = 0;
            FinalPrice = 0;
        }

        #endregion
    }
}
