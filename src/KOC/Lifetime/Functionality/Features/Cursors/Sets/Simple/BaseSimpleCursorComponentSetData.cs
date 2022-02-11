using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Collections;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Metadata;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Model;
using Appalachia.UI.Controls.Sets.Canvas;
using Appalachia.UI.Core.Components.Data;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Sets.Simple
{
    /// <summary>
    ///     Defines the metadata necessary for configuring a
    ///     <see cref="BaseSimpleCursorComponentSet{TSet, TSetData, TISetData}" />.
    /// </summary>
    /// <typeparam name="TSet">The component set.</typeparam>
    /// <typeparam name="TSetData">Styling metadata for the set.</typeparam>
    /// <typeparam name="TISetData">The interface that the style implements.</typeparam>
    [Serializable]
    [SmartLabelChildren]
    public abstract class BaseSimpleCursorComponentSetData<TSet, TSetData, TISetData> :
        BaseCanvasComponentSetData<TSet, TSetData, TISetData>,
        ISimpleCursorComponentSetData
        where TSet : BaseSimpleCursorComponentSet<TSet, TSetData, TISetData>, new()
        where TSetData : BaseSimpleCursorComponentSetData<TSet, TSetData, TISetData>, TISetData
        where TISetData : ISimpleCursorComponentSetData
    {
        static BaseSimpleCursorComponentSetData()
        {
            RegisterDependency<MainSimpleCursorLookup>(i => _mainSimpleCursorLookup = i);
        }

        #region Static Fields and Autoproperties

        private static MainSimpleCursorLookup _mainSimpleCursorLookup;

        #endregion

        #region Fields and Autoproperties

        [SerializeField] private SimpleCursors value;
        [SerializeField] private ImageData _imageData;
        [SerializeField] private SimpleCursorMetadata _metadata;

        #endregion

        public SimpleCursorMetadata Metadata
        {
            get
            {
                using (_PRF_Metadata.Auto())
                {
                    if (_metadata == null)
                    {
                        _metadata = _mainSimpleCursorLookup.Lookup.Find(_metadata.value);
                    }

                    return _metadata;
                }
            }
        }

        protected override void ApplyMetadataToComponentSet(TSet componentSet)
        {
            using (_PRF_ApplyMetadataToComponentSet.Auto())
            {
                if (_metadata != null)
                {
                    _imageData.sprite.Overriding = false;
                }

                base.ApplyMetadataToComponentSet(componentSet);

                ImageData.UpdateComponent(ref _imageData, componentSet.Image, this);

                componentSet.Image.sprite = _metadata.texture;
            }
        }

        #region ISimpleCursorComponentSetData Members

        public ImageData ImageData => _imageData;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_Metadata =
            new ProfilerMarker(_PRF_PFX + nameof(Metadata));

        #endregion
    }
}
