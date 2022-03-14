using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Controls.Simple.Contracts;
using Appalachia.UI.Functionality.Canvas.Controls.Default.Base;
using Appalachia.UI.Functionality.Images.Groups.Default;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Controls.Simple.Base
{
    /// <summary>
    ///     Defines the members necessary for creating and configuring
    ///     the components of a Image control.
    /// </summary>
    /// <typeparam name="TControl">The control.</typeparam>
    /// <typeparam name="TConfig">Configuration for the control.</typeparam>
    [Serializable]
    [SmartLabelChildren]
    public abstract class
        BaseSimpleCursorControl<TControl, TConfig> : BaseCanvasControl<TControl, TConfig>,
                                                            ISimpleCursorControl
        where TControl : BaseSimpleCursorControl<TControl, TConfig>, new()
        where TConfig: BaseSimpleCursorControlConfig<TControl, TConfig>, new()
    {
        #region Fields and Autoproperties

        [PropertyOrder(ORDER_ELEMENTS + 00)]
        [SerializeField]
        [ReadOnly]
        public ImageComponentGroup image;

        [PropertyOrder(ORDER_OBJECTS + 00)]
        [SerializeField]
        [ReadOnly]
        private GameObject _imageParent;

        #endregion

        [ShowInInspector] public Sprite Sprite => Config.Metadata.texture;

        public bool Enabled
        {
            set => GameObject.SetActive(value);
        }

        [ShowInInspector]
        public float TemplateAlpha
        {
            get => Canvas.CanvasGroup.alpha;
            set => Canvas.CanvasGroup.alpha = value;
        }

        /// <inheritdoc />
        public override void DestroySafely()
        {
            using (_PRF_DestroySafely.Auto())
            {
                if (image != null)
                {
                    image.DestroySafely();
                }

                base.DestroySafely();
            }
        }

        /// <inheritdoc />
        public override void Disable()
        {
            using (_PRF_Disable.Auto())
            {
                if (image != null)
                {
                    image.Disable();
                }

                base.Disable();
            }
        }

        /// <inheritdoc />
        public override void Enable(TConfig config)
        {
            using (_PRF_Enable.Auto())
            {
                base.Enable(config);

                if (image != null)
                {
                    image.Enable(config.Image);
                }
            }
        }

        #region ISimpleCursorControl Members

        /// <inheritdoc />
        public override void Refresh()
        {
            using (_PRF_Refresh.Auto())
            {
                base.Refresh();

                canvas.GetOrAddChild(ref _imageParent, nameof(Image), IsUI);

                ImageComponentGroup.Refresh(ref image, ImageParent, NamePrefix);
            }
        }

        public GameObject ImageParent
        {
            get => _imageParent;
            set => _imageParent = value;
        }

        public ImageComponentGroup Image => image;

        #endregion
    }
}
