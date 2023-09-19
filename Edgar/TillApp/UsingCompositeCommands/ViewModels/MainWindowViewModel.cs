using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows;
using System.Windows.Input;

namespace UsingCompositeCommands.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// The current page to display
        /// </summary>
        private ViewModelBase _currentChildView;

        public ViewModelBase CurrentChildView
        {
            get
            {
                return _currentChildView;
            }
            set
            {
                _currentChildView = value;
                onPropertyChanged(nameof(CurrentChildView));
            }
        }

        private string _title;

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                onPropertyChanged(nameof(Title));
            }
        }

        public DelegateCommand<RoutedEventArgs> BurgersCheckedCommand { get; private set; }
        public DelegateCommand<RoutedEventArgs> DrinksCheckedCommand { get; private set; }
        public DelegateCommand<RoutedEventArgs> MainMenuCheckedCommand { get; private set; }
        public DelegateCommand<RoutedEventArgs> SmileyMealCheckedCommand { get; private set; }
        public DelegateCommand<RoutedEventArgs> WrapsCheckedCommand { get; private set; }


        public MainWindowViewModel()
        {
            BurgersCheckedCommand = new DelegateCommand<RoutedEventArgs>(ExecuteShowBurgersViewCommand);
            DrinksCheckedCommand = new DelegateCommand<RoutedEventArgs>(ExecuteShowDrinksViewCommand);
            MainMenuCheckedCommand = new DelegateCommand<RoutedEventArgs>(ExecuteShowMainMenuViewCommand);
            SmileyMealCheckedCommand = new DelegateCommand<RoutedEventArgs>(ExecuteShowSmileyMealViewCommand);
            WrapsCheckedCommand = new DelegateCommand<RoutedEventArgs>(ExecuteShowWrapsViewCommand);
        }

        public void ExecuteShowBurgersViewCommand(RoutedEventArgs args)
        {
            CurrentChildView = new BurgerViewModel();
            Title = "Burgers";
        }
        public void ExecuteShowMainMenuViewCommand(RoutedEventArgs args)
        {
            CurrentChildView = new MainMenuViewModel();
            Title = "Main Menu";
        }
        public void ExecuteShowSmileyMealViewCommand(RoutedEventArgs args)
        {
            CurrentChildView = new SmileyMealViewModel();
            Title = "Smiley Meal";
        }
        public void ExecuteShowWrapsViewCommand(RoutedEventArgs args)
        {
            CurrentChildView = new WrapsViewModel();
            Title = "Wraps";
        }
        public void ExecuteShowDrinksViewCommand(RoutedEventArgs args)
        {
            CurrentChildView = new DrinksViewModel();
            Title = "Drinks";
        }
    }
}