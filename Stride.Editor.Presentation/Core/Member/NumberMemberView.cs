using Stride.Core;
using Stride.Core.Annotations;
using Stride.Editor.Commands;
using Stride.Editor.Commands.Core;
using Stride.Editor.Design.Core;
using System;
using System.Linq;
using Virtual = Stride.Editor.Presentation.VirtualDom.Controls;

namespace Stride.Editor.Presentation.Core.Member
{
    public sealed class NumberMemberView : InlineMemberView
    {
        private const int DefaultPrecision = 4;
        public NumberMemberView(IServiceRegistry services) : base(services)
        {
            dispatcher = services.GetSafeServiceAs<ICommandDispatcher>();
        }
        private ICommandDispatcher dispatcher;

        public override bool CanBeApplied(MemberViewModel viewModel)
        {
            return viewModel.TypeDescriptor.Type == typeof(int) ||
                viewModel.TypeDescriptor.Type == typeof(uint) ||
                viewModel.TypeDescriptor.Type == typeof(double) ||
                viewModel.TypeDescriptor.Type == typeof(float) ||
                viewModel.TypeDescriptor.Type == typeof(long) ||
                viewModel.TypeDescriptor.Type == typeof(ulong) ||
                viewModel.TypeDescriptor.Type == typeof(short) ||
                viewModel.TypeDescriptor.Type == typeof(ushort) ||
                viewModel.TypeDescriptor.Type == typeof(byte) ||
                viewModel.TypeDescriptor.Type == typeof(sbyte);
        }

        protected override IViewBuilder CreatePropertyView(MemberViewModel viewModel)
        {
            ComputeLimitsAndStep(viewModel, out var lower, out var upper, out var precision, out var slider);

            string format = precision == 0 ? "{0:0}" : $"{{0:0.{new string('0', precision)}}}";

            var update = CreateUpdate<double>(viewModel, v => Convert(viewModel, Math.Round(v, precision)));

            if (slider.HasValue)
                return new Virtual.StackPanel
                {
                    Orientation = Avalonia.Layout.Orientation.Horizontal,
                    Children = new IViewBuilder[]
                    {
                        new Virtual.Slider
                        {
                            Value = System.Convert.ToDouble(viewModel.Value),
                            Minimum = lower,
                            Maximum = upper,
                            SmallChange = slider.Value.SmallStep,
                            LargeChange = slider.Value.LargeStep,
                            OnValue = update,
                        },
                        new Virtual.TextBlock
                        {
                            Text = String.Format(format, viewModel.Value),
                        },
                    }
                };

            return new Virtual.NumericUpDown
            {
                Value = System.Convert.ToDouble(viewModel.Value),
                Minimum = lower,
                Maximum = upper,
                OnValue = (arg) =>
                {
                    update(arg.NewValue);
                    arg.Handled = true;
                },
                FormatString = format,
            };
        }

        private void ComputeLimitsAndStep(MemberViewModel viewModel, out double lower, out double upper, out int precision, out Slider? slider)
        {
            var attr = viewModel.MemberDescriptor.GetCustomAttributes<DataMemberRangeAttribute>(true)
                .FirstOrDefault();
            if (attr != null)
            {
                lower = attr.Minimum ?? MinValue(viewModel.TypeDescriptor.Type);
                upper = attr.Maximum ?? MaxValue(viewModel.TypeDescriptor.Type);
                precision = attr.DecimalPlaces ?? (IsIntegral(viewModel.TypeDescriptor.Type) ? 0 : DefaultPrecision);
                if (attr.SmallStep.HasValue || attr.LargeStep.HasValue)
                {
                    slider = new Slider
                    {
                        SmallStep = attr.SmallStep ?? attr.LargeStep.Value,
                        LargeStep = attr.LargeStep ?? attr.SmallStep.Value,
                    };
                }
                else slider = null;
                return;
            }

            lower = MinValue(viewModel.TypeDescriptor.Type);
            upper = MaxValue(viewModel.TypeDescriptor.Type);
            precision = IsIntegral(viewModel.TypeDescriptor.Type) ? 0 : DefaultPrecision;
            slider = null;
        }

        private bool IsIntegral(Type type)
        {
            return type != typeof(double) && type != typeof(float);
        }

        private double MinValue(Type type)
        {
            if (type == typeof(int))
                return int.MinValue;
            if (type == typeof(uint))
                return uint.MinValue;
            if (type == typeof(double))
                return double.MinValue;
            if (type == typeof(float))
                return float.MinValue;
            if (type == typeof(long))
                return long.MinValue;
            if (type == typeof(ulong))
                return ulong.MinValue;
            if (type == typeof(short))
                return short.MinValue;
            if (type == typeof(ushort))
                return ushort.MinValue;
            if (type == typeof(byte))
                return byte.MinValue;
            if (type == typeof(sbyte))
                return sbyte.MinValue;
            throw new NotSupportedException();
        }

        private double MaxValue(Type type)
        {
            if (type == typeof(int))
                return int.MaxValue;
            if (type == typeof(uint))
                return uint.MaxValue;
            if (type == typeof(double))
                return double.MaxValue;
            if (type == typeof(float))
                return float.MaxValue;
            if (type == typeof(long))
                return long.MaxValue;
            if (type == typeof(ulong))
                return ulong.MaxValue;
            if (type == typeof(short))
                return short.MaxValue;
            if (type == typeof(ushort))
                return ushort.MaxValue;
            if (type == typeof(byte))
                return byte.MaxValue;
            if (type == typeof(sbyte))
                return sbyte.MaxValue;
            throw new NotSupportedException();
        }

        private object Convert(MemberViewModel viewModel, double value)
        {
            var type = viewModel.TypeDescriptor.Type;
            if (type == typeof(int))
                return System.Convert.ToInt32(value);
            if (type == typeof(uint))
                return System.Convert.ToUInt32(value);
            if (type == typeof(double))
                return value;
            if (type == typeof(float))
                return System.Convert.ToSingle(value);
            if (type == typeof(long))
                return System.Convert.ToInt64(value);
            if (type == typeof(ulong))
                return System.Convert.ToUInt64(value);
            if (type == typeof(short))
                return System.Convert.ToInt16(value);
            if (type == typeof(ushort))
                return System.Convert.ToUInt16(value);
            if (type == typeof(byte))
                return System.Convert.ToByte(value);
            if (type == typeof(sbyte))
                return System.Convert.ToSByte(value);
            throw new NotSupportedException();
        }

        private struct Slider
        {
            public double SmallStep { get; set; }
            public double LargeStep { get; set; }
        }
    }
}
