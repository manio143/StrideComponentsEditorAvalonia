using Stride.Editor.Commands;
using Stride.Editor.Design;
using System.Threading.Tasks;

namespace Stride.Editor.Services
{
    public interface IViewUpdater
    {
        /// <summary>
        /// Invoke UI update.
        /// </summary>
        /// <param name="dispatcher">Dispatcher instance to be disabled during the UI update.</param>
        Task UpdateView(ICommandDispatcher dispatcher);
    }
}
