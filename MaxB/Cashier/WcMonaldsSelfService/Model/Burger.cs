using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcMonaldsSelfService.Model
{
    public class Burger : MenuItem
    {
        private List<string> layers = new();
        public List<string> Layers
        {
            get { return layers; }
            private set
            {
                layers = value;
            }
        }

        private List<string> layersInUse = new();
        public List<string> LayersInUse
        {
            get { return layersInUse; }
            private set
            {
                layersInUse = value;
            }
        }

        public Burger(string _name, float _price, List<string> layers) : base(_name, _price)
        {
            Layers = layers;
            LayersInUse = layers;
        }


    }
}
