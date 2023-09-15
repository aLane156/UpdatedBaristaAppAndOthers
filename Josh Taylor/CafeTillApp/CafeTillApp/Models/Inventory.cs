using System.Collections.Generic;


namespace CafeTillApp.Models
{
    /// <summary>
    /// A class that stores all of the menu items in there sub folders
    /// using a hardcoded list
    /// </summary>
    public class Inventory
    {
        /// <summary>
        /// All items for sale
        /// </summary>
        public Dictionary<string, Dictionary<string, double>> dict = new Dictionary<string, Dictionary<string, double>>
        {
            {"Coffee", new Dictionary<string, double> {{"Late", 2.50}, {"Moca", 3.00}}},
            {"Hot drink", new Dictionary<string, double> {{"Tea", 1.50}, {"Hot chocolate", 2.00}}},
            {"Cake", new Dictionary<string, double> {{"Brownie", 2.50}, {"Muffin", 2.00}}}
        };


    }
}
