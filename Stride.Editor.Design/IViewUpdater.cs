using System.Threading.Tasks;

namespace Stride.Editor.Design
{
    public interface IViewUpdater
    {
        /// <summary>
        /// Invoke UI update.
        /// </summary>
        /// <param name="dispatcher">Dispatcher instance to be disabled during the UI update.</param>
        Task UpdateView();
    }
}
