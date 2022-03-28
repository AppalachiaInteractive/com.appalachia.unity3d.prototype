using System;
using System.Collections.Generic;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Controls.Main;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Core;
using Appalachia.Utility.Async;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Widgets
{
    public sealed class StatusBarWidget : DeveloperInterfaceManager_V01.WidgetWithSingletonSubwidgets<
        IStatusBarSubwidget, IStatusBarSubwidgetMetadata, StatusBarWidget, StatusBarWidgetMetadata, StatusBarFeature,
        StatusBarFeatureMetadata>
    {
        #region Fields and Autoproperties

        private List<IStatusBarSubwidget> _leftStatusBarSubwidgets;
        private List<IStatusBarSubwidget> _rightStatusBarSubwidgets;

        public StatusBarControl statusBar;

        #endregion

        public IReadOnlyList<IStatusBarSubwidget> LeftStatusBarSubwidgets => _leftStatusBarSubwidgets;
        public IReadOnlyList<IStatusBarSubwidget> RightStatusBarSubwidgets => _rightStatusBarSubwidgets;

        public override void ValidateSubwidgets()
        {
            using (_PRF_ValidateSubwidgets.Auto())
            {
                StatusBarControl.Refresh(ref statusBar, canvas.ChildContainer, nameof(statusBar));

                RemoveIncorrectSubwidgetsFromList(
                    _leftStatusBarSubwidgets,
                    _rightStatusBarSubwidgets,
                    e => e.Metadata.Section == StatusBarSection.Left
                );

                RemoveIncorrectSubwidgetsFromList(
                    _rightStatusBarSubwidgets,
                    _leftStatusBarSubwidgets,
                    e => e.Metadata.Section == StatusBarSection.Right
                );

                EnsureSubwidgetsHaveCorrectParent(_leftStatusBarSubwidgets,  statusBar.leftStatusBar.RectTransform);
                EnsureSubwidgetsHaveCorrectParent(_rightStatusBarSubwidgets, statusBar.rightStatusBar.RectTransform);

                LayoutRebuilder.MarkLayoutForRebuild(statusBar.leftStatusBar.RectTransform);
                LayoutRebuilder.MarkLayoutForRebuild(statusBar.rightStatusBar.RectTransform);
            }
        }

        public void SortSubwidgetsByPriority()
        {
            using (_PRF_SortSubwidgetsByPriority.Auto())
            {
                SortSubwidgetsByPriority<IStatusBarSubwidget, IStatusBarSubwidgetMetadata>(_leftStatusBarSubwidgets);
                SortSubwidgetsByPriority<IStatusBarSubwidget, IStatusBarSubwidgetMetadata>(_rightStatusBarSubwidgets);
            }
        }

        /// <inheritdoc />
        protected override void EnsureWidgetIsCorrectSize()
        {
            using (_PRF_EnsureWidgetIsCorrectSize.Auto())
            {
                base.EnsureWidgetIsCorrectSize();

                var anchorMin = RectTransform.anchorMin;
                var anchorMax = RectTransform.anchorMax;

                anchorMin.x = 0.00f;
                anchorMax.x = 1.00f;

                anchorMin.y = 0.00f;
                anchorMax.y = metadata.height;

                UpdateAnchorMin(anchorMin);
                UpdateAnchorMax(anchorMax);
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                StatusBarControl.Refresh(ref statusBar, canvas.ChildContainer, nameof(statusBar));
            }
        }

        protected override void OnRegisterSubwidget(IStatusBarSubwidget subwidget)
        {
            using (_PRF_OnRegisterSubwidget.Auto())
            {
                _leftStatusBarSubwidgets ??= new List<IStatusBarSubwidget>();
                _rightStatusBarSubwidgets ??= new List<IStatusBarSubwidget>();

                switch (subwidget.Metadata.Section)
                {
                    case StatusBarSection.Left:
                        _leftStatusBarSubwidgets.Add(subwidget);

                        break;
                    case StatusBarSection.Right:
                        _rightStatusBarSubwidgets.Add(subwidget);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                _leftStatusBarSubwidgets ??= new List<IStatusBarSubwidget>();
                _rightStatusBarSubwidgets ??= new List<IStatusBarSubwidget>();

                ValidateSubwidgets();
            }
        }
    }
}
