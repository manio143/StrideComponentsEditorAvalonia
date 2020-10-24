using Stride.Core.Diagnostics;
using Stride.Editor.Design.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stride.Editor.Design.Core.Menu
{
    public class MenuProvider : IMenuProvider
    {
        private static LoggingScope Logger = LoggingScope.Global(nameof(MenuProvider));

        private Dictionary<string, (MenuItemViewModel item, MenuItemViewModel parent)> menuItemCache = new Dictionary<string, (MenuItemViewModel item, MenuItemViewModel parent)>();

        private MenuViewModel menu = new MenuViewModel { Items = new List<MenuItemViewModel>() };

        /// <inheritdoc/>
        public MenuViewModel GetMenu() => menu;

        /// <inheritdoc/>
        public void RegisterMenuItem(string parentPath, MenuItemViewModel menuItem)
        {
            Logger.Debug($"Register menu item '{menuItem.Header}' as a child of '{parentPath}'.");

            if (string.IsNullOrWhiteSpace(parentPath))
                throw new ArgumentException($"Empty {nameof(parentPath)}. For root path use '/'.");

            parentPath = parentPath.Replace("_", "");
            (MenuItemViewModel, MenuItemViewModel) cacheItem = default;
            MenuItemViewModel parent = null;

            // Get menuItem from cache or find it in the hierarchy, creating missing entries
            if (parentPath != "/" && !menuItemCache.TryGetValue(parentPath, out cacheItem))
            {
                var pathParts = parentPath.Split("/", StringSplitOptions.RemoveEmptyEntries);
                var children = menu.Items;
                MenuItemViewModel item = null;
                for (int i = 0; i < pathParts.Length; i++)
                {
                    var pathItem = pathParts[i];
                    item = children.FirstOrDefault(mi => mi.Header.Replace("_", "") == pathItem);
                    if (item == null)
                    {
                        var parentChildren = children;
                        var parentOfItem = item;
                        children = new List<MenuItemViewModel>();
                        item = new MenuItemViewModel { Header = pathItem, Items = children };
                        menuItemCache.Add("/" + string.Join("/", pathParts.Take(i + 1)), (item, parentOfItem));
                        parentChildren.Add(item);
                    }
                    else
                    {
                        children = item.Items;
                    }
                }
                parent = item;
            }
            else
            {
                parent = cacheItem.Item1;
            }

            menuItemCache.Add($"{(parentPath == "/" ? string.Empty : parentPath)}/{menuItem.Header.Replace("_", "")}", (menuItem, parent));

            if (parent != null)
            {
                if (parent.Items == null)
                    parent.Items = new List<MenuItemViewModel>();
                parent.Items.Add(menuItem);
            }
            else
                menu.Items.Add(menuItem);
        }

        /// <inheritdoc/>
        public void UnregisterMenuItem(string path)
        {
            path = path.Replace("_", "");

            if (!menuItemCache.ContainsKey(path))
                throw new InvalidOperationException($"Cannot unregister non existing menu item '{path}'");

            Logger.Debug($"Unregister menu item '{path}'.");

            var (item, parent) = menuItemCache[path];
            parent.Items.Remove(item);
            menuItemCache.Remove(path);
        }
    }
}
