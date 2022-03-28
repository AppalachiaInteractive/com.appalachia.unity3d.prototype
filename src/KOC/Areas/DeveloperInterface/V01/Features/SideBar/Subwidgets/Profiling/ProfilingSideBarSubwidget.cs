using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Core;
using Appalachia.UI.Functionality.Foldout.Control.Default;
using Appalachia.UI.Functionality.Layout.Groups.Vertical;
using Appalachia.Utility.Async;
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

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                VerticalLayoutGroupComponentGroup.Refresh(
                    ref verticalLayoutGroup,
                    canvas.ChildContainer,
                    nameof(verticalLayoutGroup)
                );

                FoldoutControl.Refresh(ref fps,    verticalLayoutGroup.gameObject, nameof(fps));
                FoldoutControl.Refresh(ref memory, verticalLayoutGroup.gameObject, nameof(memory));
                FoldoutControl.Refresh(ref audio,  verticalLayoutGroup.gameObject, nameof(audio));
            }
        }

        protected override void OnActivate()
        {
        }

        protected override void OnDeactivate()
        {
        }
    }
}
