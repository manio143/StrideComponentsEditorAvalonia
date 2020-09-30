using Avalonia;
using Avalonia.Controls;

namespace Stride.Editor.Presentation.VirtualDom
{
    public class DiffApplyer
    {
        /// <summary>
        /// Creates a concrete control from a virtual description.
        /// </summary>
        /// <typeparam name="TControl">Control to create</typeparam>
        /// <typeparam name="TVirtControl">Control description</typeparam>
        public static TControl CreateFromVirtual<TControl, TVirtControl>(TVirtControl newControl)
            where TControl : AvaloniaObject, IControl, new()
            where TVirtControl : VirtualControlBase, IVirtualControl<TControl>
        {
            var control = new TControl();
            foreach (var kvp in newControl.Patches)
            {
                control.SetValue(kvp.Key, kvp.Value.Value);
            }
            if (newControl.Content != null)
                control.SetValue(ContentControl.ContentProperty, CreateFromVirtual(newControl.Content));
            return control;
        }

        /// <summary>
        /// Applies changes in <typeparamref name="TVirtControl"/> to <paramref name="control"/> and modifies <paramref name="oldControl"/> to contain new changes.
        /// </summary>
        /// <typeparam name="TControl"></typeparam>
        /// <typeparam name="TVirtControl"></typeparam>
        /// <param name="control">Control to update</param>
        /// <param name="oldControl">Previous virtual state</param>
        /// <param name="newControl">Updates to virtual state</param>
        public static void ApplyDiff<TControl, TVirtControl>(TControl control, ref TVirtControl oldControl, TVirtControl newControl)
            where TControl : AvaloniaObject, IControl, new()
            where TVirtControl : VirtualControlBase, IVirtualControl<TControl>
        {
            foreach (var kvp in newControl.Patches)
            {
                var newPatch = kvp.Value;
                if (oldControl.Patches.TryGetValue(kvp.Key, out var oldPatch))
                {
                    if (newPatch != oldPatch)
                    {
                        control.SetValue(kvp.Key, newPatch);
                        oldControl.Patches[kvp.Key] = newPatch;
                    }
                }
                else
                {
                    control.SetValue(kvp.Key, newPatch);
                    oldControl.Patches[kvp.Key] = newPatch;
                }
            }
        }
    }
}
