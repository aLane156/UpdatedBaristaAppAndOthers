using Prism.Ioc;
using Prism.DryIoc;
using System;
using System.Windows;
using TestMVVM.View;
using Prism.Regions;
using System.Windows.Controls;
using TestMVVM.Core.Regions;

namespace TestMVVM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry) {  }
        protected override Window CreateShell() { return Container.Resolve<ShellWindow>(); }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            regionAdapterMappings.RegisterMapping(typeof(StackPanel),
                Container.Resolve<StackPanelRegionAdapter>());
        }
    }
}
