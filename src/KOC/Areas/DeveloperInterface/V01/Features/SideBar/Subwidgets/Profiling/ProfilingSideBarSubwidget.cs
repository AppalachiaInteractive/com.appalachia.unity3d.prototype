using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Core;
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

        [SerializeField] public Appalachia.UI.Controls.Sets2.Layout.Foldout.FoldoutComponentSet fps;

        [SerializeField] public Appalachia.UI.Controls.Sets2.Layout.Foldout.FoldoutComponentSet memory;

        [SerializeField] public new Appalachia.UI.Controls.Sets2.Layout.Foldout.FoldoutComponentSet audio;

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

                Appalachia.UI.Controls.Sets2.Layout.Foldout.FoldoutComponentSetData.RefreshAndUpdate(ref _metadata.fps,    ref fps,    gameObject, "FPS");
                Appalachia.UI.Controls.Sets2.Layout.Foldout.FoldoutComponentSetData.RefreshAndUpdate(ref _metadata.memory, ref memory, gameObject, "RAM");
                Appalachia.UI.Controls.Sets2.Layout.Foldout.FoldoutComponentSetData.RefreshAndUpdate(ref _metadata.audio,  ref audio,  gameObject, "Audio");

                gameObject.GetOrAddComponent(ref verticalLayoutGroup);

                VerticalLayoutGroupData.RefreshAndUpdate(
                    ref _metadata.verticalLayoutGroup,
                    _metadata,
                    verticalLayoutGroup
                );
            }
        }
    }
}
