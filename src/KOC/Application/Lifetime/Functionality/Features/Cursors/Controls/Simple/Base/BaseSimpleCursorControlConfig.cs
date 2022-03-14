using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Collections;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Controls.Simple.Contracts;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Metadata;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Model;
using Appalachia.UI.Functionality.Canvas.Controls.Default.Base;
using Appalachia.UI.Functionality.Images.Groups.Default;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Controls.Simple.Base
{
    /// <summary>
    ///     Defines the metadata necessary for configuring a
    ///     <see cref="BaseSimpleCursorControl{TControl,TConfig}" />.
    /// </summary>
    /// <typeparam name="TControl">The control.</typeparam>
    /// <typeparam name="TConfig">Configuration for the control.</typeparam>
    [Serializable]
    [SmartLabelChildren]
    public abstract class
        BaseSimpleCursorControlConfig<TControl, TConfig> : BaseCanvasControlConfig<TControl, TConfig>,
                                                                  ISimpleCursorControlConfig
        where TControl : BaseSimpleCursorControl<TControl, TConfig>, new()
        where TConfig: BaseSimpleCursorControlConfig<TControl, TConfig>, new()
    {
        static BaseSimpleCursorControlConfig()
        {
            RegisterDependency<MainSimpleCursorLookup>(i => _mainSimpleCursorLookup = i);
        }

        protected BaseSimpleCursorControlConfig()
        {
        }

        protected BaseSimpleCursorControlConfig(Object owner) : base(owner)
        {
        }

        #region Static Fields and Autoproperties

        private static MainSimpleCursorLookup _mainSimpleCursorLookup;

        #endregion

        #region Fields and Autoproperties

        [HideIf("@!ShowAllFields && (HideImage || HideAllFields)")]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private ImageComponentGroupConfig _image;

        [SerializeField] private SimpleCursors value;

        [SerializeField] private SimpleCursorMetadata _metadata;

        #endregion

        protected virtual bool HideImage => false;

        /// <inheritdoc />
        protected override void OnApply(TControl control)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(control);
                if (_metadata != null)
                {
                    _image.image.sprite.Overriding = true;
                    _image.image.sprite.Value = _metadata.texture;
                }
                
                ImageComponentGroupConfig.Apply(ref _image, Owner, control.image);

                control.enabled = Enabled;
            }
        }

        protected override void OnInitializeFields(Initializer initializer, Object owner)
        {
            using (_PRF_OnInitializeFields.Auto())
            {
                base.OnInitializeFields(initializer, owner);

                ImageComponentGroupConfig.Refresh(ref _image, owner);
            }
        }

        #region ISimpleCursorControlConfig Members

        public ImageComponentGroupConfig Image
        {
            get => _image;
            protected set => _image = value;
        }

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

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_Metadata = new ProfilerMarker(_PRF_PFX + nameof(Metadata));

        #endregion
    }
}
