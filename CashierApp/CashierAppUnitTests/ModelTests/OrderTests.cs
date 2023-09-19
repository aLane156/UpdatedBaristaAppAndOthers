namespace CashierApp.Model
{
    public class OrderTests
    {
        [Fact]
        public void CalculateTaxAmountTest()
        {
            Order order = new();

            order.OrderItems.Add(new FoodProduct("Test Food", 8.30));
            order.OrderItems.Add(new DrinkProduct("Test Drink", 7.20));

            order.CalculateFinal();

            // total: 15.5, vat: 3.1
            Assert.Equal(3.1, order.TaxAmount);
            Assert.Equal("£3.10", order.DisplayTax);
        }

        [Fact]
        public void CalculateFinalAmountTest()
        {
            Order order = new();

            order.OrderItems.Add(new FoodProduct("Test Food", 5.00));
            order.OrderItems.Add(new DrinkProduct("Test Drink", 7.20));

            order.CalculateFinal();

            // total: 12.2, vat: 2.44, final: 14.63
            Assert.Equal(14.63, order.FinalPrice);
            Assert.Equal("£14.63", order.DisplayFinal);
        }
    }
}
