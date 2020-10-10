using Stride.Core;
using System;
using System.Collections.Generic;

namespace Stride.Editor.Presentation
{
    /// <summary>
    /// View registry service providing registration of type mapping between a viewmodel and its view.
    /// </summary>
    public class ViewRegistry
    {
        private Dictionary<Type, Type> modelViewMapping = new Dictionary<Type, Type>();

        /// <summary>
        /// Register <typeparamref name="TView"/> as the view for any instance of <typeparamref name="TViewModel"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><typeparamref name="TViewModel"/> was previously registered.</exception>
        public void RegisterView<TViewModel, TView>()
            where TView : ViewBase<TViewModel>
        {
            if (modelViewMapping.ContainsKey(typeof(TViewModel)))
                throw new InvalidOperationException($"Cannot register view for a view model '{typeof(TViewModel)}' that has previously been registered.");

            modelViewMapping.Add(typeof(TViewModel), typeof(TView));
        }

        /// <summary>
        /// Removes a previously registered mapping of &lt;<typeparamref name="TViewModel"/>, <typeparamref name="TView"/>&gt;
        /// </summary>
        /// <exception cref="InvalidOperationException">This pair of types has not been registered.</exception>
        public void UnregisterView<TViewModel, TView>()
            where TView : ViewBase<TViewModel>
        {
            if (modelViewMapping.ContainsKey(typeof(TViewModel)))
            {
                var value = modelViewMapping[typeof(TViewModel)];

                if (value != typeof(TView))
                    throw new InvalidOperationException($"Inconsistency between registered view and the one being unregistered: '{value}' != '{typeof(TView)}'.");
                
                modelViewMapping.Remove(typeof(TViewModel));
            }
            else
                throw new InvalidOperationException($"No view has been registered for '{typeof(TViewModel)}'.");
        }

        /// <summary>
        /// Initializes a new instance of the registered view for the provided <paramref name="viewModel"/>, passing <paramref name="services"/> to the view's constructor.
        /// </summary>
        /// <exception cref="InvalidOperationException">No registered view for <paramref name="viewModel"/></exception>
        public IViewBase GetView(object viewModel, IServiceRegistry services)
        {
            if (modelViewMapping.TryGetValue(viewModel.GetType(), out var viewType))
            {
                return (IViewBase)Activator.CreateInstance(viewType, services);
            }

            throw new InvalidOperationException($"No view has been registered for type '{viewModel.GetType()}'");
        }
    }
}
