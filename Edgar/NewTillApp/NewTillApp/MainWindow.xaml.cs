// MainWindow.xaml.cs
using System.ComponentModel;
using System.Windows;
using NewTillApp.View;
using NewTillApp.Views;
using Prism.Ioc;
using Prism.Unity;


namespace NewTillApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }

    public class Bootstrapper : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Page1>();
            containerRegistry.RegisterForNavigation<Page2>();
        }

        protected override void InitializeShell(Window shell)
        {
            Application.Current.MainWindow = shell;
            Application.Current.MainWindow.Show();
        }
    }
}
