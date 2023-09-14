using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeTillApp.Models
{
    /// <summary>
    /// A class that stores all of the menu items in there sub folders
    /// using a hardcoded list
    /// </summary>
    public class Inventory
    {
        public Dictionary<string, string[]> dict = new Dictionary<string, string[]>
        {
            {"Coffee", new string[] {"late", "moca"}},
            {"Hot drink", new string[] {"tea", "hot chocolate"}},
            {"Cake", new string[] {"brownie", "muffin"}}
        };

    }
}
