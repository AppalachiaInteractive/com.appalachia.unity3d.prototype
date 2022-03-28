using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Controls.Main.Contracts;
using Appalachia.UI.ControlModel.Controls;
using Appalachia.UI.ControlModel.Controls.Default;
using Appalachia.UI.Functionality.Layout.Groups.Vertical;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Controls.Main.Base
{
    /// <summary>
    ///     Defines the members necessary for creating and configuring
    ///     the components of a ActivityBar control.
    /// </summary>
    /// <typeparam name="TControl">The control.</typeparam>
    /// <typeparam name="TConfig">Configuration for the control.</typeparam>
    [Serializable]
    [SmartLabelChildren]
    public abstract class BaseActivityBarControl<TControl, TConfig> : AppaUIControl<TControl, TConfig>,
                                                                      IActivityBarControl
        where TControl : BaseActivityBarControl<TControl, TConfig>, new()
        where TConfig : BaseActivityBarControlConfig<TControl, TConfig>, new()
    {
        #region Fields and Autoproperties

        [FoldoutGroup(GROUP_COMP)]
        [PropertyOrder(ORDER_ELEMENTS + 00)]
        [SerializeField]
        [ReadOnly]
        public VerticalLayoutGroupComponentGroup topActivityBar;

        [FoldoutGroup(GROUP_COMP)]
        [PropertyOrder(ORDER_ELEMENTS + 01)]
        [SerializeField]
        [ReadOnly]
        public VerticalLayoutGroupComponentGroup bottomActivityBar;

        [FoldoutGroup(GROUP_COMP)]
        [PropertyOrder(ORDER_OBJECTS + 00)]
        [SerializeField]
        [ReadOnly]
        private GameObject _topActivityBarParent;

        [FoldoutGroup(GROUP_COMP)]
        [PropertyOrder(ORDER_OBJECTS + 01)]
        [SerializeField]
        [ReadOnly]
        private GameObject _bottomActivityBarParent;

        #endregion

        /// <inheritdoc />
        public override void DestroySafely()
        {
            using (_PRF_DestroySafely.Auto())
            {
                if (topActivityBar != null)
                {
                    topActivityBar.DestroySafely();
                }

                if (bottomActivityBar != null)
                {
                    bottomActivityBar.DestroySafely();
                }
            }
        }

        /// <inheritdoc />
        public override void Disable()
        {
            using (_PRF_Disable.Auto())
            {
                if (topActivityBar != null)
                {
                    topActivityBar.Disable();
                }

                if (bottomActivityBar != null)
                {
                    bottomActivityBar.Disable();
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

                if (topActivityBar != null)
                {
                    topActivityBar.Enable(config.TopActivityBar);
                }

                if (bottomActivityBar != null)
                {
                    bottomActivityBar.Enable(config.BottomActivityBar);
                }
            }
        }

        #region IActivityBarControl Members

        public VerticalLayoutGroupComponentGroup BottomActivityBar => bottomActivityBar;

        public VerticalLayoutGroupComponentGroup TopActivityBar => topActivityBar;

        public GameObject BottomActivityBarParent
        {
            get => _bottomActivityBarParent;
            set => _bottomActivityBarParent = value;
        }

        public GameObject TopActivityBarParent
        {
            get => _topActivityBarParent;
            set => _topActivityBarParent = value;
        }

        /// <inheritdoc />
        protected override void OnRefresh()
        {
            using (_PRF_OnRefresh.Auto())
            {
                base.OnRefresh();

                _topActivityBarParent = gameObject;
                _bottomActivityBarParent = gameObject;

                VerticalLayoutGroupComponentGroup.Refresh(
                    ref topActivityBar,
                    TopActivityBarParent,
                    nameof(topActivityBar)
                );
                VerticalLayoutGroupComponentGroup.Refresh(
                    ref bottomActivityBar,
                    BottomActivityBarParent,
                    nameof(bottomActivityBar)
                );

                topActivityBar.transform.SetSiblingIndex(0);
                bottomActivityBar.transform.SetSiblingIndex(1);
            }
        }

        #endregion
    }
}
