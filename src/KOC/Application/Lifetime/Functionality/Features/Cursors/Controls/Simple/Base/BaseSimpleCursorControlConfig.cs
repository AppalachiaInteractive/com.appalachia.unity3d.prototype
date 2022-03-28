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
using UnityEngine.Serialization;
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
    public abstract class BaseSimpleCursorControlConfig<TControl, TConfig> : BaseCanvasControlConfig<TControl, TConfig>,
                                                                             ISimpleCursorControlConfig
        where TControl : BaseSimpleCursorControl<TControl, TConfig>, new()
        where TConfig : BaseSimpleCursorControlConfig<TControl, TConfig>, new()
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
        public ImageComponentGroupConfig image;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public SimpleCursors value;

        [FormerlySerializedAs("_metadata")]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public SimpleCursorMetadata metadata;

        #endregion

        protected virtual bool HideImage => false;

        /// <inheritdoc />
        protected override void OnApply(TControl control)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(control);
                if (metadata != null)
                {
                    image.image.sprite.Overriding = true;
                    image.image.sprite.Value = Metadata.texture;
                }

                image.Apply(control.image);

                control.enabled = Enabled;
            }
        }

        protected override void OnInitializeFields(Initializer initializer)
        {
            using (_PRF_OnInitializeFields.Auto())
            {
                base.OnInitializeFields(initializer);

                ImageComponentGroupConfig.Refresh(ref image, Owner);
            }
        }

        protected override void OnRefresh(Object owner)
        {
            using (_PRF_OnRefresh.Auto())
            {
                base.OnRefresh(owner);
                
                ImageComponentGroupConfig.Refresh(ref image, Owner);
            }
        }

        protected override void SubscribeResponsiveConfigs()
        {
            using (_PRF_SubscribeResponsiveConfigs.Auto())
            {
                base.SubscribeResponsiveConfigs();

                image.SubscribeToChanges(OnChanged);
                if (Metadata != null)
                {
                    Metadata.SubscribeToChanges(OnChanged);
                }
            }
        }

        protected override void UnsuspendResponsiveConfigs()
        {
            using (_PRF_UnsuspendResponsiveConfigs.Auto())
            {
                base.UnsuspendResponsiveConfigs();

                image.UnsuspendChanges();
                if (Metadata != null)
                {
                    Metadata.UnsuspendChanges();
                }
            }
        }
        
        protected override void SuspendResponsiveConfigs()
        {
            using (_PRF_SuspendResponsiveConfigs.Auto())
            {
                base.SuspendResponsiveConfigs();

                image.SuspendChanges();
                if (Metadata != null)
                {
                    Metadata.SuspendChanges();
                }
            }
        }
        #region ISimpleCursorControlConfig Members

        public ImageComponentGroupConfig Image
        {
            get => image;
            protected set => image = value;
        }

        public SimpleCursorMetadata Metadata
        {
            get
            {
                using (_PRF_Metadata.Auto())
                {
                    if (metadata == null)
                    {
                        metadata = _mainSimpleCursorLookup.Lookup.Find(value);
                    }

                    return metadata;
                }
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_Metadata = new ProfilerMarker(_PRF_PFX + nameof(Metadata));

        #endregion
    }
}
