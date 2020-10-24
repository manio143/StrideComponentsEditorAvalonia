using Stride.Core;
using Stride.Editor.Design;
using Stride.Editor.Design.AssetBrowser;
using Stride.Editor.Design.Core;
using Stride.Editor.Presentation.Core.Docking;
using Stride.Editor.Services;
using System.Threading.Tasks;

namespace Stride.Editor.Menu
{
    public class OpenCommand : MenuCommandBase
    {
        public OpenCommand(IServiceRegistry services) : base(services)
        {
            Session = services.GetSafeServiceAs<Session>();
        }

        private Session Session { get; }

        protected override async Task ExecuteAsync(object parameter)
        {
            Session.EditorViewModel.LoadingStatus = new LoadingStatus(LoadingStatus.LoadingMode.Indeterminate);
            await Services.GetService<IViewUpdater>().UpdateView();

            await Session.LoadProject(@"D:\Documents\Stride Projects\MinimalTestProject\MinimalTestProject.sln");

            Session.EditorViewModel.LoadingStatus = null;
            var browser = new AssetBrowserViewModel(Session.PackageSession);
            await Services.GetService<TabManager>().CreateToolTab(browser);
            await Services.GetService<IViewUpdater>().UpdateView();
        }
    }
}
