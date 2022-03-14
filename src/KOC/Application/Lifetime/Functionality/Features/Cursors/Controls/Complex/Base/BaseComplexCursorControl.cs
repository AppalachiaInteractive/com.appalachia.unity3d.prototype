using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Controls.Complex.Contracts;
using Appalachia.UI.Animations;
using Appalachia.UI.Functionality.Images.Groups.Default;
using Appalachia.UI.Functionality.MultiPartCanvas;
using Appalachia.Utility.Extensions;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Controls.Complex.Base
{
    /// <summary>
    ///     Defines the members necessary for creating and configuring
    ///     the components of a Cursor control.
    /// </summary>
    /// <typeparam name="TControl">The control.</typeparam>
    /// <typeparam name="TConfig">Configuration for the control.</typeparam>
    [Serializable]
    [SmartLabelChildren]
    public abstract class BaseComplexCursorControl<TControl, TConfig> :
        BaseMultiPartCanvasControl<TControl, TConfig, ImageComponentGroup, ImageComponentGroup.List,
            ImageComponentGroupConfig, ImageComponentGroupConfig.List>,
        IComplexCursorControl
        where TControl : BaseComplexCursorControl<TControl, TConfig>
        where TConfig : BaseComplexCursorControlConfig<TControl, TConfig>, new()

    {
        #region Fields and Autoproperties

        [SerializeField] private Animator _animator;
#if UNITY_EDITOR
        [SerializeField] private AnimationRemapper _animationRemapper;
#endif

        #endregion

        /// <inheritdoc />
        public override void DestroySafely()
        {
            using (_PRF_DestroySafely.Auto())
            {
                base.DestroySafely();

                if (_animator)
                {
                    _animator.DestroySafely();
                }
#if UNITY_EDITOR
                if (_animationRemapper)
                {
                    _animationRemapper.DestroySafely();
                }
#endif
            }
        }

        /// <inheritdoc />
        public override void Disable()
        {
            using (_PRF_Disable.Auto())
            {
                base.Disable();

                if (_animator)
                {
                    _animator.enabled = false;
                }

#if UNITY_EDITOR
                if (_animationRemapper)
                {
                    _animationRemapper.enabled = false;
                }
#endif
            }
        }

        /// <inheritdoc />
        public override void Enable(TConfig config)
        {
            using (_PRF_Enable.Auto())
            {
                base.Enable(config);

                if (_animator)
                {
                    _animator.enabled = true;
                }

#if UNITY_EDITOR
                if (_animationRemapper)
                {
                    _animationRemapper.enabled = true;
                }
#endif
            }
        }

        public override GameObject GetMultiPartParent()
        {
            using (_PRF_GetMultiPartParent.Auto())
            {
                return _animator.gameObject;
            }
        }

        #region IComplexCursorControl Members

        /// <inheritdoc />
        public override void Refresh()
        {
            using (_PRF_Refresh.Auto())
            {
                base.Refresh();

                GameObject.GetOrAddComponent(ref _animator);
                GameObject.GetOrAddComponent(ref _animationRemapper);
            }
        }

        public Animator Animator => _animator;

#if UNITY_EDITOR
        public AnimationRemapper AnimationRemapper => _animationRemapper;
#endif

        #endregion
    }
}
