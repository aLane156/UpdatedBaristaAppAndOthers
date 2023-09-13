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
        public int maxCount;
        private int count;
        public int Count
        {
            get { return count; }
            set
            {
                if (value <= maxCount)
                {
                    count = value;
                }
            }
        }

        public LooseMeats(string _name, float _price, int _count) : base(_name, _price)
        {
            Count = _count;
        }
    }
}
