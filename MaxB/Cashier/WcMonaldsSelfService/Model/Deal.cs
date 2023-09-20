using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcMonaldsSelfService.Model
{
    class Deal
    {
        private MenuItem[] dealComponents;
        public MenuItem[] DealComponents
        {
            get => dealComponents;
            set
            {
                dealComponents = value;
            }
        }

        private float discount;
        public float Discount
        {
            get => discount;
            set
            {
                discount = value;
            }
        }
        public Deal (MenuItem[] item, float _Discount)
        {
            DealComponents = item;
            Discount = _Discount;
        }

        public bool CheckIfApplies(MenuItem[] items, out float _discount)
        {
            int correctitems = 0;
            foreach (MenuItem component in DealComponents) 
            {
                //applies = items.Contains(component) && applies;
                foreach (MenuItem item in items)
                {
                    if (item.Name == component.Name)
                    {
                        correctitems++;
                        break;
                    }
                }
            }
            if (correctitems == DealComponents.Length)
            {
                _discount = Discount;
            } else
            {
                _discount = 0;
            }
            return correctitems == DealComponents.Length;
        }
    }
}
