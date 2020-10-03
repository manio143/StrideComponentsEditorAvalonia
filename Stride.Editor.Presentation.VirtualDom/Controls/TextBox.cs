using Avalonia.FuncUI.DSL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stride.Editor.Presentation.VirtualDom.Controls
{
    public class TextBox : ViewBuilder<Avalonia.Controls.TextBox>
    {
        public string Text
        {
            set { Property(Avalonia.Controls.TextBox.TextProperty, value); }
        }

        public Action<string> OnText
        {
            set { Subscribe(Avalonia.Controls.TextBox.TextProperty, value, SubPatchOptions.Always); }
        }
    }
}
