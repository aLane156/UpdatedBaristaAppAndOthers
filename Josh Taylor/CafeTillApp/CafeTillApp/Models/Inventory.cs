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
            {"Coffee", new Dictionary<string, double> {{"Late", 2.50}, {"Mocha", 3.00}, {"Espresso", 2.00}, {"Cappuccino", 2.75}}},
            {"Hot drink", new Dictionary<string, double> {{"Tea", 1.50}, {"Hot chocolate", 2.00}, {"Chai Latte", 2.50}, {"Matcha Latte", 3.00}}},
            {"Cake", new Dictionary<string, double> {{"Brownie", 2.50}, {"Muffin", 2.00}, {"Cheesecake", 3.50}, {"Carrot Cake", 3.00}}},
            {"Sandwich", new Dictionary<string, double> {{"Ham and Cheese", 5.00}, {"Tuna Melt", 5.50}, {"Chicken Caesar Wrap", 6.00}}},
            {"Salad", new Dictionary<string, double> {{"Caesar Salad", 7.00}, {"Greek Salad", 7.50}, {"Cobb Salad", 8.00}}}
        };



    }
}
