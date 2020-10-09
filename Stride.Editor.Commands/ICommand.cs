using System;
using System.Collections.Generic;
using System.Text;

namespace Stride.Editor.Commands
{
    public interface ICommand
    {
        /// <summary>
        /// Executes command with the provided <paramref name="context"/>.
        /// </summary>
        void Execute(object context);
    }

    public interface ICommand<T> : ICommand
    {
        /// <summary>
        /// Executes command with the provided <paramref name="context"/>.
        /// </summary>
        void Execute(T context);
        void ICommand.Execute(object context) => Execute((T)context);
    }
}
