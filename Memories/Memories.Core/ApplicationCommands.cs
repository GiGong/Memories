using Prism.Commands;

namespace Memories.Core
{
    public interface IApplicationCommands
    {
        CompositeCommand HideShellCommand { get; }
        CompositeCommand ShowShellCommand { get; }
        CompositeCommand DrawControlCommand { get; }
    }

    public class ApplicationCommands : IApplicationCommands
    {
        public CompositeCommand HideShellCommand { get; } = new CompositeCommand();

        public CompositeCommand ShowShellCommand { get; } = new CompositeCommand();

        public CompositeCommand DrawControlCommand { get; } = new CompositeCommand();
    }
}
