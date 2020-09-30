using System;
using System.Collections.Generic;
using System.Text;

namespace Stride.Editor.Presentation
{
    public interface IViewBase
    {
        IViewBuilder CreateView(object viewModel);
    }
}
