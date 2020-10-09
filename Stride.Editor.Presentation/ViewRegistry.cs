using Stride.Core;
using System;
using System.Collections.Generic;

namespace Stride.Editor.Presentation
{
    public class ViewRegistry
    {
        private Dictionary<Type, Type> modelViewMapping = new Dictionary<Type, Type>();

        public void RegisterView<TViewModel, TView>()
            where TView : ViewBase<TViewModel>
        {
            modelViewMapping.Add(typeof(TViewModel), typeof(TView));
        }

        public IViewBase GetView(object viewModel, IServiceRegistry services)
        {
            if (modelViewMapping.TryGetValue(viewModel.GetType(), out var viewType))
            {
                return (IViewBase)Activator.CreateInstance(viewType, services);
            }

            throw new ApplicationException($"No view has been registered for type '{viewModel.GetType()}'");
        }
    }
}
