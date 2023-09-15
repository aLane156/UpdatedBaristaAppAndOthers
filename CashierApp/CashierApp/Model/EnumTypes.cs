using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierApp.Model
{
    /// <summary>
    /// Enum for storing all types of LogTypes.
    /// </summary>
    public enum LogType
    {
        DEBUG,
        INFO,
        WARNING,
        ERROR,
    }

    public enum ProductType
    {
        Food,
        Drink,
        Dessert
    }

    public enum DrinkSizes
    {
        Small,
        Medium,
        Large
    }
}
