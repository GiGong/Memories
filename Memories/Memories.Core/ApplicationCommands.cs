﻿using Prism.Commands;

namespace Memories.Core
{
    public interface IApplicationCommands
    {
        CompositeCommand HideShellCommand { get; }
        CompositeCommand ShowShellCommand { get; }
    }

    public class ApplicationCommands : IApplicationCommands
    {
        public CompositeCommand HideShellCommand { get; } = new CompositeCommand();

        public CompositeCommand ShowShellCommand { get; } = new CompositeCommand();
    }
}