using Stride.Core;
using Stride.Editor.Commands;
using Stride.Editor.Design;
using Stride.Editor.Services;
using System.Threading.Tasks;

namespace Stride.Editor.Menu
{
    public class SaveAllCommand : MenuCommandBase
    {
        public SaveAllCommand(IServiceRegistry services) : base(services)
        {
            Session = services.GetSafeServiceAs<Session>();
            AssetManager = services.GetSafeServiceAs<IAssetManager>();
            CommandDispatcher = services.GetSafeServiceAs<ICommandDispatcher>();

            Session.ProjectLoaded += (_) => OnCanExecuteChanged();
        }

        public Session Session { get; }
        public IAssetManager AssetManager { get; }
        public ICommandDispatcher CommandDispatcher { get; }

        public override bool CanExecute(object parameter) => Session.PackageSession != null;

        protected override Task ExecuteAsync(object parameter)
        {
            CommandDispatcher.Dispatch(new Commands.AssetManager.SaveAllCommand(),
                new Commands.AssetManager.SaveAllCommand.Context
                {
                    AssetManager = AssetManager,
                });
            return Task.FromResult(false);
        }
    }
}
