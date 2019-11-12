using Prism.Commands;

namespace Memories.Core
{
    public interface IApplicationCommands
    {
        CompositeCommand HideShellCommand { get; }
        CompositeCommand ShowShellCommand { get; }
        CompositeCommand DrawControlCommand { get; }

        CompositeCommand NewBookCommand { get; }
        CompositeCommand SaveCommand { get; }
        CompositeCommand SaveAsCommand { get; }
        CompositeCommand LoadCommand { get; }

        CompositeCommand CloseEditBookViewCommand { get; }

        //Should this stay here?
        CompositeCommand PageMoveCommand { get; }
    }

    public class ApplicationCommands : IApplicationCommands
    {
        public CompositeCommand HideShellCommand { get; } = new CompositeCommand();

        public CompositeCommand ShowShellCommand { get; } = new CompositeCommand();

        public CompositeCommand DrawControlCommand { get; } = new CompositeCommand();

        public CompositeCommand NewBookCommand { get; } = new CompositeCommand();

        public CompositeCommand SaveCommand { get; } = new CompositeCommand();

        public CompositeCommand SaveAsCommand { get; } = new CompositeCommand();

        public CompositeCommand LoadCommand { get; } = new CompositeCommand();

        public CompositeCommand CloseEditBookViewCommand { get; } = new CompositeCommand();

        public CompositeCommand PageMoveCommand { get; } = new CompositeCommand();
    }
}
