using System;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Core;
using Appalachia.UI.Controls.Components.Buttons;
using Appalachia.UI.Controls.Sets.Layout.Foldout;
using Appalachia.UI.Core.Components.Subsets;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Profiling
{
    public class
        ProfilingSideBarSubwidget : SideBarSubwidget<ProfilingSideBarSubwidget, ProfilingSideBarSubwidgetMetadata>
    {
        #region Fields and Autoproperties

        [SerializeField] public FoldoutComponentSet fps;

        [SerializeField] public FoldoutComponentSet memory;

        [SerializeField] public new FoldoutComponentSet audio;

        [SerializeField] public VerticalLayoutGroupSubset verticalLayoutGroup;

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
