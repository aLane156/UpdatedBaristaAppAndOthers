using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Xps.Serialization;

namespace WcMonaldsSelfService.Model
{
    public class LooseMeats : MenuItem
    {
        private int count;
        public int Count
        {
            get { return count; }
            set
            {
                count = value;
            }
        }

        private float pricePerItem;

        public LooseMeats(string _name, float _price, int _count) : base(_name, _price)
        {
            Count = _count;
            pricePerItem = _price / _count;
        }

        /// <summary>
        /// Changes the number of items in this object and updates the price accordingly
        /// </summary>
        /// <param name="newCount">New number of items</param>
        /// <returns>If the change was successful</returns>
        public bool ChangeNo(int newCount)
        {
            Count = newCount;
            Price = pricePerItem * count;
            return (count == newCount);
        }
    }
}
