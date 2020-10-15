using Avalonia.Layout;
using Stride.Core;
using Stride.Core.Diagnostics;
using Stride.Editor.Design;
using Stride.Editor.Design.Core;
using Stride.Editor.Design.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Virtual = Stride.Editor.Presentation.VirtualDom.Controls;

namespace Stride.Editor.Presentation.Core
{
    public class MemberViewProvider : ViewBase<MemberViewModel>, IMemberViewProvider<IViewBuilder>
    {
        private static LoggingScope Logger = LoggingScope.Global(nameof(MemberViewProvider));
        private readonly List<MemberView> MemberViews = new List<MemberView>();
        
        public MemberViewProvider(IServiceRegistry services) : base(services)
        {
        }

        public override IViewBuilder CreateView(MemberViewModel viewModel)
        {
            // last registered has priority
            for(int i = MemberViews.Count-1; i >= 0; i--)
            {
                if (MemberViews[i].CanBeApplied(viewModel))
                    return MemberViews[i].CreateView(viewModel);
            }

            return UnsupportedView(viewModel);
        }

        private IViewBuilder UnsupportedView(MemberViewModel viewModel)
        {
            return new Virtual.DockPanel
            {
                Children = new IViewBuilder[]
                {
                    new Virtual.TextBlock
                    {
                        Text = viewModel.Name,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                    },
                    new Virtual.TextBlock
                    {
                        Text = viewModel.Value?.ToString() ?? "(null)",
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Center,
                        IsEnabled = viewModel.Value != null,
                    },
                }
            };
        }

        public void RegisterMemberView(Func<MemberViewModel, bool> canBeApplied, IView<MemberViewModel, IViewBuilder> view)
        {
            Logger.Info($"Register MemberView {view.GetType()}.");
            MemberViews.Add(new MemberView
            {
                CanBeApplied = canBeApplied,
                View = view,
            });
        }

        public void UnregisterMemberView<TView>() where TView : IView<MemberViewModel, IViewBuilder>
        {
            var memberView = MemberViews.FirstOrDefault(m => m.View is TView);
            if (memberView.View != null)
            {
                Logger.Info($"Unegister MemberView {memberView.View.GetType()}");
                MemberViews.Remove(memberView);
            }
        }

        private struct MemberView : IView<MemberViewModel, IViewBuilder>
        {
            public Func<MemberViewModel, bool> CanBeApplied { get; set; }
            public IView<MemberViewModel, IViewBuilder> View { get; set; }
            public IViewBuilder CreateView(MemberViewModel viewModel)
                => View.CreateView(viewModel);
        }
    }
}
