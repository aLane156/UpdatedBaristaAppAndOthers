using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcMonaldsSelfService.Model
{
    public class Burger : MenuItem
    {
        private List<string> layers = new List<string>();
        public List<string> Layers
        {
            get { return layers; }
            private set
            {
                layers = value;
            }
        }

        public Burger(string _name, float _price, List<string> layers) : base(_name, _price)
        {
            Layers = layers;
        }
    }
}
