﻿using Memories.Modules.NewBook.ViewModels;
using Memories.Modules.NewBook.Views;
using Memories.Services;
using Memories.Services.Interfaces;
using Prism.Ioc;
using Prism.Modularity;

namespace Memories.Modules.NewBook
{
    public class NewBookModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<NewBookView, NewBookViewVM>();
            containerRegistry.RegisterForNavigation<InputBookInfoView, InputBookInfoViewVM>();
            containerRegistry.RegisterForNavigation<BookLayoutSelectView, BookLayoutSelectViewVM>();

            containerRegistry.RegisterSingleton<IBookLayoutService, BookLayoutService>();
        }
    }
}