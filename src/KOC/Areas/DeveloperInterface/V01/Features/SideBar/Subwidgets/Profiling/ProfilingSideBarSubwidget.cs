using System;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Core;
using Appalachia.UI.Functionality.Buttons.Components;
using Appalachia.UI.Functionality.Foldout.Control.Default;
using Appalachia.UI.Functionality.Layout.Groups.Vertical;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Profiling
{
    public class
        ProfilingSideBarSubwidget : SideBarSubwidget<ProfilingSideBarSubwidget, ProfilingSideBarSubwidgetMetadata>
    {
        #region Fields and Autoproperties

        [SerializeField] public FoldoutControl fps;

        [SerializeField] public FoldoutControl memory;

        [SerializeField] public new FoldoutControl audio;

        [SerializeField] public VerticalLayoutGroupComponentGroup verticalLayoutGroup;

        #endregion

        protected override bool ShowsTooltip => false;

        public override string GetDevTooltipText()
        {
            throw new NotImplementedException();
        }

        public override void OnClicked()
        {
            using (_PRF_OnClicked.Auto())
            {
            }
        }

        protected override AppaButton GetTooltipTarget()
        {
            throw new NotImplementedException();
        }

        protected override void OnActivate()
        {
        }

        protected override void OnDeactivate()
        {
        }
    }
}
