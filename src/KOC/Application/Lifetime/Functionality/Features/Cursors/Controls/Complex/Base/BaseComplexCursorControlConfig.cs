using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Controls.Complex.Contracts;
using Appalachia.UI.Functionality.Animation;
using Appalachia.UI.Functionality.Images.Groups.Default;
using Appalachia.UI.Functionality.MultiPartCanvas;
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

        [SerializeField] private AnimatorConfig _animatorData;

        #endregion

        /// <inheritdoc />
        protected override void OnApply(TControl control)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(control);

                AnimatorConfig.RefreshAndApply(ref _animatorData, Owner, control.Animator);
            }
        }

        protected override void OnInitializeFields(Initializer initializer, Object owner)
        {
            using (_PRF_OnInitializeFields.Auto())
            {
                base.OnInitializeFields(initializer, owner);

                AnimatorConfig.Refresh(ref _animatorData, owner);
            }
        }

        #region IComplexCursorControlConfig Members

        public AnimatorConfig AnimatorConfig
        {
            get => _animatorData;
            protected set => _animatorData = value;
        }

        #endregion
    }
}
