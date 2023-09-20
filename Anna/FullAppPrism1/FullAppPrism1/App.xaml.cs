﻿using FullAppPrism1.Modules.ModuleName;
using FullAppPrism1.Services;
using FullAppPrism1.Services.Interfaces;
using FullAppPrism1.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace FullAppPrism1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ModuleNameModule>();
        }
    }
}
