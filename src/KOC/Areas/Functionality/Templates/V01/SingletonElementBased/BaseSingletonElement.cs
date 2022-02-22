using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Templates.V01.SingletonElementBased
{
    [CallStaticConstructorInEditor]
    public abstract class BaseSingletonElement<TWidget, TWidgetMetadata, TElement, TIElement, TElementMetadata,
                                          TFeature, TFeatureMetadata, TAreaManager,
                                          TAreaMetadata> : SingletonAppalachiaBehaviour<TElement>, IAvailabilityMarker
        where TWidget : ElementBasedWidget<TWidget, TWidgetMetadata, TElement, TIElement, TElementMetadata,
            TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TWidgetMetadata : ElementBasedWidgetMetadata<TWidget, TWidgetMetadata, TElement, TIElement,
            TElementMetadata, TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TElement : BaseSingletonElement<TWidget, TWidgetMetadata, TElement, TIElement, TElementMetadata,
            TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>, TIElement
        where TIElement : class, IAvailabilityMarker
        where TElementMetadata : BaseSingletonElementMetadata<TWidget, TWidgetMetadata, TElement, TIElement,
            TElementMetadata, TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeature : ElementBasedFeature<TWidget, TWidgetMetadata, TElement, TIElement, TElementMetadata,
            TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeatureMetadata : ElementBasedFeatureMetadata<TWidget, TWidgetMetadata, TElement, TIElement,
            TElementMetadata, TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
        #region Fields and Autoproperties

        [FormerlySerializedAs("_modalData")]
        [SerializeField]
        private TElementMetadata _elementData;

        #endregion

        public TElementMetadata ElementData => _elementData;

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(
                    this,
                    nameof(_elementData),
                    _elementData == null,
                    () =>
                    {
                        var assetName = $"{gameObject.name}{typeof(TElementMetadata).Name}";
                        _elementData = AppalachiaObject.LoadOrCreateNew<TElementMetadata>(
                            assetName,
                            ownerType: AppalachiaRepository.PrimaryOwnerType
                        );
                    }
                );
            }
        }
    }
}
