using Stride.Core;
using Stride.Core.Reflection;
using System.Diagnostics;

namespace Stride.Editor.Presentation
{
    internal class Module
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            // Make sure that this assembly is registered
            AssemblyRegistry.Register(typeof(Module).Assembly, AssemblyCommonCategories.Assets);
        }
    }
}
