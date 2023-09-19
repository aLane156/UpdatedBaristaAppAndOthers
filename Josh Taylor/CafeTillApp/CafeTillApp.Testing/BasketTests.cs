using CafeTillApp.ViewModels;
using Prism.Events;
using System.Collections.ObjectModel;

namespace CafeTillApp.Testing
{
    public class BasketTests
    {
        [Fact]
        public void CalculateTotal_ShouldCalculateCorrectTotal()
        {
            // Arrange
            //var sharedBasket = new SharedBasket();
            MainWindowViewModel.SharedBasket.Basket = new ObservableCollection<string>();
            MainWindowViewModel.SharedBasket.Basket.Add("Cheesecake\n£3.50");
            MainWindowViewModel.SharedBasket.Basket.Add("Cheesecake\n£3.50");

            // Create an instance of IEventAggregator
            IEventAggregator eventAggregator = new EventAggregator(); // replace with actual implementation if different

            // Create an instance of BasketViewModel
            var basketViewModel = new BasketViewModel(eventAggregator);

            // Act
            var result = basketViewModel.CalculateTotal();

            // Assert
            Assert.Equal("Total: £7.00", result);
        }
    }
}