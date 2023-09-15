using CashierApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shell;

namespace CashierApp.ViewModel
{
    public class SelectedEventAggregator
    {
        public static Action<Product> OnMessageTransmitted;

        public static void BroadCast(Product message) 
        {
            OnMessageTransmitted(message);
        }
    }

    public static class DispatchHandler
    {
        /// <summary>
        /// Function for handling async in MVVM.
        /// </summary>
        public static void HandleAwait(Application app, Action f)
        {
            app.Dispatcher.InvokeAsync(f);
        }

        public static void HandleAwait(Application app, Func<object, ObservableCollection<Product>> f, ref ObservableCollection<FoodProduct> collectionData)
        {
            app.Dispatcher.InvokeAsync(async () => 
            {
                var result = await Task.Run(() => f(new()));
            });
        }
    }
}
