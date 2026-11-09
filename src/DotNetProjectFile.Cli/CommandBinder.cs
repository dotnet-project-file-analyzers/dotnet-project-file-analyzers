using System.CommandLine;

namespace DotNetProjectFile.Cli;

/// <summary>Fluent extesions on <see cref="Command" />.</summary>
public static class CommandBinder
{
    extension(Command command)
    {
        /// <see cref="Command.SetAction(Action{ParseResult})" />
        public Command Bind(Action<ParseResult> execution)
        {
            command.SetAction(execution);
            return command;
        }
    }
}
