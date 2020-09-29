using Stride.Editor.Design;

namespace Stride.Editor.Commands
{
    public interface IEditorCommand<TEditor> where TEditor : IAssetEditor
    {
        void Apply(TEditor editor);
    }
}
