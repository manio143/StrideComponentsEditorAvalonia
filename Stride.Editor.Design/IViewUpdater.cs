using System.Threading.Tasks;

namespace Stride.Editor.Design
{
    public interface IViewUpdater
    {
        /// <summary>
        /// Invoke UI update.
        /// </summary>
        Task UpdateView();
    }
}
