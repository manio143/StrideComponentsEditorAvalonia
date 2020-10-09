using Stride.Core;
using Stride.Editor.Design.Core;
using System;

namespace Stride.Editor.Presentation.Core
{
    public class EditorView : ViewBase<EditorViewModel>
    {
        public EditorView(IServiceRegistry services) : base(services)
        {
        }

        public override IViewBuilder CreateView(EditorViewModel viewModel)
        {
            throw new NotImplementedException();
        }
    }
}
