using System.Collections.Generic;
using System.Linq;
using Microsoft.FSharp.Core;
using Stride.Core;
using Stride.Core.Yaml;
using static Avalonia.FuncUI.Types;

namespace Stride.Editor.Presentation.VirtualDom
{
    public static class DebugViewPrinter
    {
        public static string PrintView(IView view)
        {
            var viewObject = BuildViewObject(view);
            return new Serializer().Serialize(viewObject);
        }

        private static ViewObject BuildViewObject(IView view)
        {
            return new ViewObject
            {
                Name = view.ViewType.Name,
                Attributes = view.Attrs.Select<IAttr, (string, object)>(a =>
                {
                    if (FSharpOption<Content>.None != a.Content)
                    {
                        var property = a.Content.Value.accessor.IsAvaloniaProperty
                            ? (a.Content.Value.accessor as Accessor.AvaloniaProperty).Item.Name
                            : (a.Content.Value.accessor as Accessor.InstanceProperty).Item.name;
                        if (a.Content.Value.content is ViewContent.Single single)
                        {
                            var value = FSharpOption<IView>.None == single.Item
                                ? null
                                : single.Item.Value;
                            return (property, BuildViewObject(value));
                        }
                        else
                        {
                            var multi = a.Content.Value.content as ViewContent.Multiple;
                            return (property, multi.Item.Select(BuildViewObject).ToArray());
                        }
                    }
                    if (FSharpOption<Property>.None != a.Property)
                    {
                        var property = a.Property.Value.accessor.IsAvaloniaProperty
                            ? (a.Property.Value.accessor as Accessor.AvaloniaProperty).Item.Name
                            : (a.Property.Value.accessor as Accessor.InstanceProperty).Item.name;
                        return (property, a.Property.Value.value?.ToString());
                    }
                    if (FSharpOption<Subscription>.None != a.Subscription)
                    {
                        return (a.Subscription.Value.name, a.Subscription.Value.funcType.Name);
                    }
                    return ("invalid", null);
                }).ToArray(),
            };
        }

        [DataContract("DebugViewObject")]
        public class ViewObject
        {
            public string Name { get; set; }
            public (string, object)[] Attributes { get; set; }
        }

        private class Serializer : YamlSerializer
        {
            public string Serialize<T>(T obj)
                => GetYamlSerializer().Serialize(obj);
        }
    }
}
