using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Controls.Main.Contracts;
using Appalachia.UI.ControlModel.Controls.Default;
using Appalachia.UI.Functionality.Buttons.Components;
using Appalachia.UI.Functionality.Layout.Groups.Horizontal;
using Appalachia.UI.Functionality.Tooltips.Controls.Base;
using Appalachia.UI.Functionality.Tooltips.Styling;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Controls.Main.Base
{
    /// <summary>
    ///     Defines the members necessary for creating and configuring
    ///     the components of a StatusBar control.
    /// </summary>
    /// <typeparam name="TControl">The control.</typeparam>
    /// <typeparam name="TConfig">Configuration for the control.</typeparam>
    [Serializable]
    [SmartLabelChildren]
    public abstract class BaseStatusBarControl<TControl, TConfig> : AppaUIControl<TControl, TConfig>,
                                                                    IStatusBarControl
        where TControl : BaseStatusBarControl<TControl, TConfig>, new()
        where TConfig : BaseStatusBarControlConfig<TControl, TConfig>, new()
    {

        #region Fields and Autoproperties

        [FoldoutGroup(GROUP_COMP)]
        [PropertyOrder(ORDER_ELEMENTS + 00)]
        [SerializeField]
        [ReadOnly]
        public HorizontalLayoutGroupComponentGroup leftStatusBar;

        [FoldoutGroup(GROUP_COMP)]
        [PropertyOrder(ORDER_ELEMENTS + 01)]
        [SerializeField]
        [ReadOnly]
        public HorizontalLayoutGroupComponentGroup rightStatusBar;

        [FoldoutGroup(GROUP_COMP)]
        [PropertyOrder(ORDER_OBJECTS + 00)]
        [SerializeField]
        [ReadOnly]
        private GameObject _leftStatusBarParent;

        [FoldoutGroup(GROUP_COMP)]
        [PropertyOrder(ORDER_OBJECTS + 01)]
        [SerializeField]
        [ReadOnly]
        private GameObject _rightStatusBarParent;

        #endregion

        /// <inheritdoc />
        public override void DestroySafely()
        {
            using (_PRF_DestroySafely.Auto())
            {
                if (leftStatusBar != null)
                {
                    leftStatusBar.DestroySafely();
                }

                if (rightStatusBar != null)
                {
                    rightStatusBar.DestroySafely();
                }
            }
        }

        /// <inheritdoc />
        public override void Disable()
        {
            using (_PRF_Disable.Auto())
            {
                if (leftStatusBar != null)
                {
                    leftStatusBar.Disable();
                }

                if (rightStatusBar != null)
                {
                    rightStatusBar.Disable();
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

                if (leftStatusBar != null)
                {
                    leftStatusBar.Enable(config.LeftStatusBar);
                }

                if (rightStatusBar != null)
                {
                    rightStatusBar.Enable(config.RightStatusBar);
                }
            }
        }

        /// <inheritdoc />
        protected override void OnRefresh()
        {
            using (_PRF_OnRefresh.Auto())
            {
                base.OnRefresh();

                _leftStatusBarParent = gameObject;
                _rightStatusBarParent = gameObject;

                HorizontalLayoutGroupComponentGroup.Refresh(
                    ref leftStatusBar,
                    LeftStatusBarParent,
                    nameof(leftStatusBar)
                );
                HorizontalLayoutGroupComponentGroup.Refresh(
                    ref rightStatusBar,
                    RightStatusBarParent,
                    nameof(rightStatusBar)
                );

                leftStatusBar.transform.SetSiblingIndex(0);
                rightStatusBar.transform.SetSiblingIndex(1);
            }
        }

        #region IStatusBarControl Members

        public HorizontalLayoutGroupComponentGroup RightStatusBar => rightStatusBar;

        public HorizontalLayoutGroupComponentGroup LeftStatusBar => leftStatusBar;

        public GameObject RightStatusBarParent
        {
            get => _rightStatusBarParent;
            set => _rightStatusBarParent = value;
        }

        public GameObject LeftStatusBarParent
        {
            get => _leftStatusBarParent;
            set => _leftStatusBarParent = value;
        }

        #endregion
    }
}
