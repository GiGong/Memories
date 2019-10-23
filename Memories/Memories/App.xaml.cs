using Memories.Core;
using Memories.Core.Controls;
using Memories.Modules.EditBook;
using Memories.Modules.NewBook;
using Memories.Modules.Start;
using Memories.Services;
using Memories.Services.Interfaces;
using Memories.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Unity;
using System;
using System.Reflection;
using System.Windows;

namespace Memories
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
#if DEBUG
#else
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
#endif

            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IBookService, BookService>();
            containerRegistry.RegisterSingleton<IFileService, FileService>();
            containerRegistry.RegisterSingleton<IFolderService, FolderService>();

            containerRegistry.RegisterDialogWindow<MMDialogWindow>();

            containerRegistry.RegisterSingleton<IApplicationCommands, ApplicationCommands>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<StartModule>();
            moduleCatalog.AddModule<NewBookModule>();
            moduleCatalog.AddModule<EditBookModule>();
        }

        /// <summary>
        /// Change naming convention in ViewModelLocator Autowire
        /// </summary>
        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewName = viewType.FullName.Replace(".Views.", ".ViewModels.");
                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                var viewModelName = $"{viewName}VM, {viewAssemblyName}";
                return Type.GetType(viewModelName);
            });
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MMMessageBox.Show("In UnhandledException" + Environment.NewLine + ((Exception)e.ExceptionObject).Message);

            throw (Exception)e.ExceptionObject;
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MMMessageBox.Show("In DispatcherUnhandledException" + Environment.NewLine + e.Exception.Message);

            throw e.Exception;
        }
    }
}
