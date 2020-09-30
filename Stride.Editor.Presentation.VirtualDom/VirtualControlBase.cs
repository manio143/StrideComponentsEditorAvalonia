using Avalonia;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Stride.Editor.Presentation.VirtualDom
{
    public abstract class VirtualControlBase : IVirtualControl, IEquatable<VirtualControlBase>
    {
        internal struct Patch : IEquatable<Patch>
        {
            public object Value { get; set; }
            public IEqualityComparer<object> Comparer { get; set; }

            public bool Equals(Patch other)
                => Comparer.Equals(Value, other.Value);
        }

        internal struct VirtualContent : IEquatable<VirtualContent>
        {
            public Type TargetType { get; set; }
            public Type VirtualType { get; set; }
            public VirtualControlBase Content { get; set; }

            public bool Equals(VirtualContent other)
                => Content.Equals(other.Content);
        }

        internal Dictionary<AvaloniaProperty, Patch> Patches = new Dictionary<AvaloniaProperty, Patch>();

        internal VirtualContent Content;
        internal Tuple<AvaloniaProperty, IList<VirtualContent>> MultiContent;

        protected void SetProperty<TValue>(AvaloniaProperty<TValue> property, TValue value)
        {
            SetProperty(property, value, DefaultEqualityComparer<TValue>.Instance);
        }

        protected void SetProperty<TValue>(AvaloniaProperty<TValue> property, TValue value, IEqualityComparer<object> comparer)
        {
            Patches.Add(property, new Patch { Value = value, Comparer = comparer });
        }

        public bool Equals(VirtualControlBase obj)
            => ListEqualityComparer<Patch>.Instance.Equals(Patches.Values.ToList(), obj.Patches.Values.ToList())
                && Content.Equals(obj.Content)
                && MultiContent == null ? obj.MultiContent == null :
                    MultiContent.Item1 == obj.MultiContent.Item1
                    && ListEqualityComparer<VirtualContent>.Instance.Equals(MultiContent.Item2, obj.MultiContent.Item2);

        public class DefaultEqualityComparer<T> : IEqualityComparer<object>
        {
            public static DefaultEqualityComparer<T> Instance = new DefaultEqualityComparer<T>();
            public new bool Equals([AllowNull] object x, [AllowNull] object y)
            {
                var xx = (T)x;
                var yy = (T)y;
                return EqualityComparer<T>.Default.Equals(xx, yy);
            }

            public int GetHashCode([DisallowNull] object obj)
            {
                var xx = (T)obj;
                return EqualityComparer<T>.Default.GetHashCode(xx);
            }
        }
        public class ListEqualityComparer<T> : IEqualityComparer<object>
        {
            public static ListEqualityComparer<T> Instance = new ListEqualityComparer<T>();
            public new bool Equals([AllowNull] object x, [AllowNull] object y)
            {
                var xx = (IList<T>)x;
                var yy = (IList<T>)y;
                if (xx.Count != yy.Count)
                    return false;
                for (int i = 0; i < xx.Count; i++)
                    if (!EqualityComparer<T>.Default.Equals(xx[i], yy[i]))
                        return false;
                return true;
            }

            public int GetHashCode([DisallowNull] object obj)
            {
                var xx = (IList<T>)obj;
                return xx.Aggregate(0, (acc, x) => acc + x.GetHashCode());
            }
        }
    }
}
