using CashierApp.Model.Types;
using CashierApp.Model;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace CashierApp.ViewModel
{
    /// <summary>
    /// Class for sending products to the MainWindowViewModel using events.
    /// </summary>
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
