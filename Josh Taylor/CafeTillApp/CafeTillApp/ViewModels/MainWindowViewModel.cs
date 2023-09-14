using CafeTillApp.Views;
using Prism.Events;
using Prism.Mvvm;
using System.Windows.Controls;

namespace CafeTillApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Till App";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private readonly IEventAggregator _eventAggregator;

        private UserControl _leftSide;
        public UserControl LeftSide
        {
            get { return _leftSide; }
            set { SetProperty(ref _leftSide, value); }
        }

        private UserControl _rightSide;
        public UserControl RightSide
        {
            get { return _rightSide; }
            set { SetProperty(ref _rightSide, value); }
        }

        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ChangeViewEvent>().Subscribe(OnChangeView);

            // Set initial views
            LeftSide = new MenuView();
            RightSide = new BasketView();
        }

        private void OnChangeView(UserControl newView)
        {
            // Change LeftSide based on some condition
            LeftSide = newView;
        }
    }
}
