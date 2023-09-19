// MainWindowViewModel.cs
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using NewTillApp.Views;

namespace NewTillApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;

        public DelegateCommand SwitchToPage2Command { get; }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            SwitchToPage2Command = new DelegateCommand(SwitchToPage2);
        }

        private void SwitchToPage2()
        {
            regionManager.RequestNavigate("ContentRegion", "Page2");
        }
    }
}
