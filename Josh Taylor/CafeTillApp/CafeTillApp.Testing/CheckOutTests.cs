using CafeTillApp.ViewModels;
using CafeTillApp.Views;
using Prism.Events;
using System.Collections.ObjectModel;
using Moq;

namespace CafeTillApp.Testing
{
    public class CheckOutTestsFixture
    {
        public IEventAggregator EventAggregator { get; private set; }
        public Mock<ChangeViewEvent> ChangeViewEvent { get; private set; }

        public CheckOutTestsFixture()
        {
            var mockEventAggregator = new Mock<IEventAggregator>();
            var mockChangeViewEvent = new Mock<ChangeViewEvent>();
            mockEventAggregator.Setup(ea => ea.GetEvent<ChangeViewEvent>()).Returns(mockChangeViewEvent.Object);

            EventAggregator = mockEventAggregator.Object;
            ChangeViewEvent = mockChangeViewEvent;
        }
    }

    public class CheckOutTests : IClassFixture<CheckOutTestsFixture>
    {
        private readonly CheckOutTestsFixture _fixture;

        public CheckOutTests(CheckOutTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [StaFact]
        public void TestEnterKey_ValidTips()
        {
            // Arrange
            var viewModel = new CheckOutViewModel(_fixture.EventAggregator);
            viewModel.Tips = "10.00";

            // Act
            viewModel.EnterKeyPressed();

            // Assert
            Assert.Single(MainWindowViewModel.SharedBasket.Basket);
            Assert.Equal("Tip \n£10.00", MainWindowViewModel.SharedBasket.Basket[0]);
            _fixture.ChangeViewEvent.Verify(e => e.Publish(It.IsAny<CheckOutView>()), Times.Once);
        }
    }

}