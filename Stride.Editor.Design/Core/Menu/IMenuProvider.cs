namespace Stride.Editor.Design.Core.Menu
{
    public interface IMenuProvider
    {
        /// <summary>
        /// Sets up the <see cref="MenuViewModel"/> according to the registered menus.
        /// </summary>
        MenuViewModel GetMenu();

        /// <summary>
        /// Registers the <paramref name="menuItem"/> as a child of an item described by the path. If the parent described by the <paramref name="parentPath"/> doesn't exist, it will be created.
        /// </summary>
        /// <remarks><paramref name="parentPath"/> should not contain the underscore character</remarks>
        /// <param name="parentPath">Path to the parent menu item. Starts with "/".</param>
        /// <param name="menuItem">Menu item to be registered.</param>
        void RegisterMenuItem(string parentPath, MenuItemViewModel menuItem);

        /// <summary>
        /// Unregisters the menu item under specified <paramref name="path"/>
        /// </summary>
        void UnregisterMenuItem(string path);
    }
}
