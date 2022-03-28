using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Functionality.Animation.Groups;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Controls.Complex.Contracts;
using Appalachia.UI.Functionality.Images.Groups.Default;
using Appalachia.UI.Functionality.MultiPartCanvas;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
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

        [FoldoutGroup(GROUP_COMP)]
        [PropertyOrder(ORDER_ELEMENTS + 00)]
        [SerializeField]
        [ReadOnly]
        public AnimatorComponentGroup animator;

        [FoldoutGroup(GROUP_COMP)]
        [PropertyOrder(ORDER_OBJECTS + 00)]
        [SerializeField]
        [ReadOnly]
        private GameObject _animatorParent;

        #endregion

        /// <inheritdoc />
        public override void DestroySafely()
        {
            using (_PRF_DestroySafely.Auto())
            {
                base.DestroySafely();

                if (animator)
                {
                    animator.DestroySafely();
                }
            }
        }

        /// <inheritdoc />
        public override void Disable()
        {
            using (_PRF_Disable.Auto())
            {
                base.Disable();

                if (animator)
                {
                    animator.enabled = false;
                }
            }
        }

        /// <inheritdoc />
        public override void Enable(TConfig config)
        {
            using (_PRF_Enable.Auto())
            {
                base.Enable(config);

                if (animator)
                {
                    animator.enabled = true;
                }
            }
        }

        public override GameObject GetMultiPartParent()
        {
            using (_PRF_GetMultiPartParent.Auto())
            {
                _animatorParent = gameObject;

                AnimatorComponentGroup.Refresh(ref animator, _animatorParent, nameof(animator));

                return animator.gameObject;
            }
        }

        #region IComplexCursorControl Members

        /// <inheritdoc />
        protected override void OnRefresh()
        {
            using (_PRF_OnRefresh.Auto())
            {
                base.OnRefresh();

                _animatorParent = gameObject;

                AnimatorComponentGroup.Refresh(ref animator, _animatorParent, nameof(animator));
            }
        }

        public AnimatorComponentGroup Animator => animator;

        public GameObject AnimatorParent => _animatorParent;

        #endregion
    }
}
