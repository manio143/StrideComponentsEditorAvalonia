using Stride.Core;
using Stride.Editor.Commands;
using Stride.Editor.Commands.AssetManager;
using Stride.Editor.Design;
using Stride.Editor.Services;
using System.Threading.Tasks;

namespace Stride.Editor.Menu
{
    public class SaveCommand : MenuCommandBase
    {
        public SaveCommand(IServiceRegistry services) : base(services)
        {
            Session = services.GetSafeServiceAs<Session>();
            AssetManager = services.GetSafeServiceAs<IAssetManager>();
            CommandDispatcher = services.GetSafeServiceAs<ICommandDispatcher>();

            AssetManager.AssetOpened += (_, __) => OnCanExecuteChanged();
        }

        public Session Session { get; }
        public IAssetManager AssetManager { get; }
        public ICommandDispatcher CommandDispatcher { get; }

        public override bool CanExecute(object parameter)
            => Session.EditorViewModel.ActiveEditor != null;

        protected override Task ExecuteAsync(object parameter)
        {
            CommandDispatcher.Dispatch(new SaveAssetCommand(), new SaveAssetCommand.Context
            {
                Asset = Session.EditorViewModel.ActiveEditor.Asset,
                AssetManager = AssetManager,
            });
            return Task.FromResult(false);
        }
    }
}
