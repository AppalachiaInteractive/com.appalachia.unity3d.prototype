using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Functionality.Animation.Groups;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Controls.Complex.Contracts;
using Appalachia.UI.Functionality.Images.Groups.Default;
using Appalachia.UI.Functionality.MultiPartCanvas;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Controls.Complex.Base
{
    /// <summary>
    ///     Defines the metadata necessary for configuring a
    ///     <see cref="BaseComplexCursorControl{TControl, TConfig}" />.
    /// </summary>
    /// <typeparam name="TControl">The control.</typeparam>
    /// <typeparam name="TConfig">Configuration for the control.</typeparam>
    [Serializable]
    [SmartLabelChildren]
    public abstract class BaseComplexCursorControlConfig<TControl, TConfig> :
        BaseMultiPartCanvasControlConfig<TControl, TConfig, ImageComponentGroup, ImageComponentGroup.List,
            ImageComponentGroupConfig, ImageComponentGroupConfig.List>,
        IComplexCursorControlConfig
        where TControl : BaseComplexCursorControl<TControl, TConfig>
        where TConfig : BaseComplexCursorControlConfig<TControl, TConfig>, new()

    {
        #region Fields and Autoproperties

        [HideIf("@!ShowAllFields && (HideAnimator || HideAllFields)")]
        [PropertyOrder(ORDER_ROOT - 10)]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public AnimatorComponentGroupConfig animator;

        #endregion

        protected virtual bool HideAnimator => false;

        /// <inheritdoc />
        protected override void OnApply(TControl control)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(control);

                animator.Apply(control.animator);
            }
        }

        protected override void OnInitializeFields(Initializer initializer)
        {
            using (_PRF_OnInitializeFields.Auto())
            {
                base.OnInitializeFields(initializer);

                AnimatorComponentGroupConfig.Refresh(ref animator, Owner);
            }
        }

        protected override void OnRefresh(Object owner)
        {
            using (_PRF_OnRefresh.Auto())
            {
                base.OnRefresh(owner);
                
                AnimatorComponentGroupConfig.Refresh(ref animator, Owner);
            }
        }

        protected override void SubscribeResponsiveConfigs()
        {
            using (_PRF_SubscribeResponsiveConfigs.Auto())
            {
                base.SubscribeResponsiveConfigs();

                animator.SubscribeToChanges(OnChanged);
            }
        }
        
        protected override void SuspendResponsiveConfigs()
        {
            using (_PRF_SuspendResponsiveConfigs.Auto())
            {
                base.SuspendResponsiveConfigs();

                animator.SuspendChanges();
            }
        }
        protected override void UnsuspendResponsiveConfigs()
        {
            using (_PRF_UnsuspendResponsiveConfigs.Auto())
            {
                base.UnsuspendResponsiveConfigs();

                animator.UnsuspendChanges();
            }
        }

        #region IComplexCursorControlConfig Members

        public AnimatorComponentGroupConfig Animator
        {
            get => animator;
            protected set => animator = value;
        }

        #endregion
    }
}
