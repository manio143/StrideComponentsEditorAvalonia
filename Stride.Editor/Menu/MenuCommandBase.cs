using Stride.Core;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Stride.Editor.Menu
{
    public abstract class MenuCommandBase : ICommand
    {
        protected MenuCommandBase(IServiceRegistry services)
        {
            Services = services;
        }

        protected IServiceRegistry Services { get; }

        public event EventHandler CanExecuteChanged;

        protected void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public virtual bool CanExecute(object parameter) => true;

        public async void Execute(object parameter) => await ExecuteAsync(parameter);

        protected abstract Task ExecuteAsync(object parameter);
    }
}
