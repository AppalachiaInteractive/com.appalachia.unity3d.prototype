/*
using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Collections;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Metadata;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Model;
using Appalachia.UI.Controls.Sets2.Canvases.Canvas;
using Appalachia.UI.Core.Components.Data;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Sets.Simple
{
    /// <summary>
    ///     Defines the metadata necessary for configuring a
    ///     <see cref="BaseSimpleCursorComponentSet{TComponentSet, TComponentSetData, TIComponentSetData}" />.
    /// </summary>
    /// <typeparam name="TComponentSet">The component set.</typeparam>
    /// <typeparam name="TComponentSetData">Styling metadata for the set.</typeparam>
    /// <typeparam name="TIComponentSetData">The interface that the style implements.</typeparam>
    [Serializable]
    [SmartLabelChildren]
    public abstract class BaseSimpleCursorComponentSetData<TComponentSet, TComponentSetData, TIComponentSetData> :
        BaseCanvasComponentSetData<TComponentSet, TComponentSetData, TIComponentSetData>,
        ISimpleCursorComponentSetData
        where TComponentSet : BaseSimpleCursorComponentSet<TComponentSet, TComponentSetData, TIComponentSetData>, new()
        where TComponentSetData : BaseSimpleCursorComponentSetData<TComponentSet, TComponentSetData, TIComponentSetData>, TIComponentSetData
        where TIComponentSetData : ISimpleCursorComponentSetData
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

        /// <inheritdoc />
        public override void ApplyToComponentSet(TComponentSet componentSet)
        {
            using (_PRF_ApplyToComponentSet.Auto())
            {
                if (_metadata != null)
                {
                    _imageData.sprite.Overriding = false;
                }

                base.ApplyToComponentSet(componentSet);

                ImageData.RefreshAndUpdate(ref _imageData, this, componentSet.Image);

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
*/
