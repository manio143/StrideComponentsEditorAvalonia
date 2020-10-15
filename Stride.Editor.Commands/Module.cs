using Stride.Core;
using Stride.Core.Reflection;
using Stride.Editor.Design;

namespace Stride.Editor.Commands
{
    internal class Module
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            // Make sure that this assembly is registered
            AssemblyRegistry.Register(typeof(Module).Assembly,
                AssemblyCommonCategories.Assets,
                EditorAssemblyCategory.Editor);
        }
    }
}
