using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Xps.Serialization;

namespace WcMonaldsSelfService.Model
{
    public enum DrinkSize
    {
        small,
        medium,
        large,
    }

    public class Drink : MenuItem
    {
        public DrinkSize Size { get; private set; }

        private DrinkSize[] acceptedSizes;
        public DrinkSize[] AcceptedSizes
        {
            get { return acceptedSizes; }
            set
            {
                if (value.Length > 0 && value.Length < 4)
                {
                    acceptedSizes = value;
                }
            }
        }

        public float BasePrice { get; private set; }

        public Drink(string _name, float _price, DrinkSize[] _acceptedSizes) : base(_name, _price)
        {
            AcceptedSizes = _acceptedSizes;
            if (AcceptedSizes.Length != 0)
            {
                Size = AcceptedSizes[0];
            }
            BasePrice = _price;
        }

        public void SetSize(DrinkSize _size)
        {
            Size = _size;
            Price = 1f + (0.5f * (int)Size) * BasePrice;
        }
    }
}
