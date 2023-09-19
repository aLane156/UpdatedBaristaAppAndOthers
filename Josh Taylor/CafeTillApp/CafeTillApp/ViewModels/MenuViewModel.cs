using CafeTillApp.Models;
using System.Collections.Generic;
using Prism.Mvvm;
using Prism.Commands;
using Prism.Events;
using CafeTillApp.Views;
using System.Windows.Media;
using System;
using System.Windows;

namespace CafeTillApp.ViewModels
{
    public class MenuViewModel : BindableBase
    {
        private DelegateCommand<string> _buttonCommand;
        public DelegateCommand<string> ButtonCommand =>
            _buttonCommand ?? (_buttonCommand = new DelegateCommand<string>(ExecuteButtonCommand));

        // Binding the current dictionary to the buttons they relate to
        private Dictionary<string, Dictionary<string, double>> _currentDictionary;
        public Dictionary<string, Dictionary<string, double>> CurrentDictionary
        {
            get { return _currentDictionary; }
            set { SetProperty(ref _currentDictionary, value); }
        }

        public Brush RandomColor
        {
            get
            {
                var colorList = new List<Color>
                {
                    Color.FromRgb(124, 63, 88), // Corresponds to #7C3F58
                    Color.FromRgb(235, 107, 111), // Corresponds to #EB6B6F
                    Color.FromRgb(249, 168, 117), // Corresponds to #F9A875
                    Color.FromRgb(255, 246, 211)  // Corresponds to #FFF6D3
                };
                var random = new Random();
                var color = colorList[random.Next(colorList.Count)];
                return new SolidColorBrush(color);
            }
        }
        public double RandomSize
        {
            get
            {
                var sizeList = new List<double> { 120, 150, 180 };
                var random = new Random();
                return sizeList[random.Next(sizeList.Count)];
            }
        }

        public Thickness RandomMargin
        {
            get
            {
                var marginList = new List<Thickness> { new Thickness(25), new Thickness(30), new Thickness(35) };
                var random = new Random();
                return marginList[random.Next(marginList.Count)];
            }
        }

        private readonly IEventAggregator _eventAggregator;

        public MenuViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            Inventory inventory = new Inventory();

            CurrentDictionary = new Dictionary<string, Dictionary<string, double>>();
            CurrentDictionary = inventory.dict;
        }

        /// <summary>
        /// Carries option on and pushes to next subMenuView
        /// </summary>
        /// <param name="option"></param>
        void ExecuteButtonCommand(string option)
        {
            MainWindowViewModel.SharedOption.Option = option;

            var newView = new SubMenuView();
            _eventAggregator.GetEvent<ChangeViewEvent>().Publish(newView);
        }
    }
}
