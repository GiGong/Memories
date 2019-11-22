using CefSharp;
using CefSharp.Wpf;
using Memories.Core;
using Memories.Core.Controls;
using Memories.Modules.EditBook;
using Memories.Modules.Facebook;
using Memories.Modules.NewBook;
using Memories.Modules.SelectImage;
using Memories.Modules.Start;
using Memories.Services;
using Memories.Services.Interfaces;
using Memories.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Unity;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;

namespace Memories
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public App()
        {
            //Add Custom assembly resolver

            //Note: If you want language of exception message is english, use follow
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");

            //Any CefSharp references have to be in another method with NonInlining
            // attribute so the assembly rolver has time to do it's thing.
            AppDomain.CurrentDomain.AssemblyResolve += Resolver;
            InitializeCefSharp();
        }

        protected override Window CreateShell()
        {
#if DEBUG
#else
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
#endif
            return Container.Resolve<MainWindow>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            ClearFacebookToken();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IBookService, BookService>();
            containerRegistry.RegisterSingleton<IFileService, FileService>();
            containerRegistry.RegisterSingleton<IFolderService, FolderService>();

            containerRegistry.RegisterSingleton<IFacebookService, FacebookService>();

            containerRegistry.RegisterDialogWindow<MMDialogWindow>();

            containerRegistry.RegisterSingleton<IApplicationCommands, ApplicationCommands>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<StartModule>();
            moduleCatalog.AddModule<NewBookModule>();
            moduleCatalog.AddModule<EditBookModule>();
            moduleCatalog.AddModule<SelectImageModule>();
            moduleCatalog.AddModule<FacebookModule>();
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

        #region Custom Exception Handler

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show("In UnhandledException" + Environment.NewLine + ((Exception)e.ExceptionObject).Message);

            ClearFacebookToken();

            throw (Exception)e.ExceptionObject;

        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("In DispatcherUnhandledException" + Environment.NewLine + e.Exception.Message);

            ClearFacebookToken();

            throw e.Exception;
        }

        #endregion Custom Exception Handler

        private void ClearFacebookToken()
        {
            Container.Resolve<IFacebookService>().ClearAuthorize();
        }

        #region Cef Sharp

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void InitializeCefSharp()
        {
            //Perform dependency check to make sure all relevant resources are in our output directory.
            var settings = new CefSettings
            {
                Locale = "ko"
            };

            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
        }

        // Will attempt to load missing assembly from either x86 or x64 subdir
        // Required by CefSharp to load the unmanaged dependencies when running using AnyCPU
        private static Assembly Resolver(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("CefSharp"))
            {
                string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
                string archSpecificPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                       Environment.Is64BitProcess ? "x64" : "x86",
                                                       assemblyName);

                return File.Exists(archSpecificPath)
                           ? Assembly.LoadFile(archSpecificPath)
                           : null;
            }

            return null;
        }

        #endregion Cef Sharp
    }
}
