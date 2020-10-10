using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Stride.Core;
using Stride.Editor.Commands;
using Stride.Editor.Design.Core;
using Stride.Editor.Presentation;
using Stride.Editor.Presentation.Core;
using Stride.Editor.Services;
using System;

namespace Stride.Editor.Avalonia
{
    public class MainWindow : Window
    {
        private ServiceRegistry Services = new ServiceRegistry();
        private Session Session;
        public MainWindow()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            Content = new ContentControl();

            Session = new Session();

            Session.EditorViewModel = new EditorViewModel();
            Session.Services = Services;
            
            Services.AddService<Session>(Session);
            Services.AddService<ViewRegistry>(new ViewRegistry());
            Services.AddService<IViewUpdater>(new ViewUpdater(Services, this.Content as ContentControl));
            Services.AddService<IUndoService>(new UndoService());
            Services.AddService<ICommandDispatcher>(new CommandDispatcher(Services));
            Services.AddService<IMemberViewProvider<IViewBuilder>>(new MemberViewProvider(Services));

            // TODO: register plugins (remember to allow loading plugins dynamically)
            // TODO: render view
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
