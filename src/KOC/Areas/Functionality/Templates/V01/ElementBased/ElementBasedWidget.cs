using System.Collections.Generic;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Templates.V01.ElementBased
{
    public abstract class ElementBasedWidget<TWidget, TWidgetMetadata, TElement, TElementMetadata, TFeature,
                                             TFeatureMetadata, TAreaManager, TAreaMetadata> : AreaWidget<
        TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TWidget : ElementBasedWidget<TWidget, TWidgetMetadata, TElement, TElementMetadata, TFeature,
            TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TWidgetMetadata : ElementBasedWidgetMetadata<TWidget, TWidgetMetadata, TElement,
            TElementMetadata, TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TElement : BaseElement<TWidget, TWidgetMetadata, TElement, TElementMetadata,
            TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>, IEnableNotifier
        where TElementMetadata : BaseElementMetadata<TWidget, TWidgetMetadata, TElement, TElementMetadata,
            TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeature : ElementBasedFeature<TWidget, TWidgetMetadata, TElement, TElementMetadata, TFeature,
            TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeatureMetadata : ElementBasedFeatureMetadata<TWidget, TWidgetMetadata, TElement,
            TElementMetadata, TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
        #region Fields and Autoproperties

        protected List<TElement> _elements;

        #endregion

        public IReadOnlyList<TElement> Elements => _elements;

        /// <summary>
        ///     Adds the specified element to the widget's collection.
        /// </summary>
        /// <param name="element">The element to add.</param>
        public void RegisterElement(TElement element)
        {
            using (_PRF_RegisterElement.Auto())
            {
                _elements ??= new();

                for (var i = 0; i < _elements.Count; i++)
                {
                    if (_elements[i] == element)
                    {
                        return;
                    }
                }

                _elements.Add(element);

                OnRegisterElement(element);
            }
        }

        /// <summary>
        ///     Implements any feature specific handling of the element when it is registered.
        /// </summary>
        /// <param name="element"></param>
        protected abstract void OnRegisterElement(TElement element);

        #region Profiling

        protected static readonly ProfilerMarker _PRF_OnRegisterElement =
            new ProfilerMarker(_PRF_PFX + nameof(OnRegisterElement));

        private static readonly ProfilerMarker _PRF_RegisterElement =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterElement));

        #endregion
    }
}
