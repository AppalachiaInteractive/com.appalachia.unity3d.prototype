using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Core;
using Appalachia.UI.Controls.Sets.Layout.Foldout;
using Appalachia.UI.Core.Components.Data;
using Appalachia.Utility.Extensions;
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

        [SerializeField] public VerticalLayoutGroup verticalLayoutGroup;

        #endregion

        public override void OnClicked()
        {
            using (_PRF_OnClicked.Auto())
            {
            }
        }

        protected override void OnUpdateSubwidget()
        {
            using (_PRF_OnUpdateSubwidget.Auto())
            {
                base.OnUpdateSubwidget();

                FoldoutComponentSetData.RefreshAndApply(ref Metadata.fps,    ref fps,    gameObject, "FPS");
                FoldoutComponentSetData.RefreshAndApply(ref Metadata.memory, ref memory, gameObject, "RAM");
                FoldoutComponentSetData.RefreshAndApply(ref Metadata.audio,  ref audio,  gameObject, "Audio");

                gameObject.GetOrAddComponent(ref verticalLayoutGroup);

                VerticalLayoutGroupData.RefreshAndApply(
                    ref Metadata.verticalLayoutGroup,
                    Metadata,
                    verticalLayoutGroup
                );
            }
        }
    }
}
