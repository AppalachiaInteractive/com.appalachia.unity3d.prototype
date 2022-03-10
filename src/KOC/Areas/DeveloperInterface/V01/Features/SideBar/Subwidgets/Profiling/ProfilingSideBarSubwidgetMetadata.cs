using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Core;
using Appalachia.UI.Controls.Sets.Layout.Foldout;
using Appalachia.UI.Controls.Sets.Layout.Foldout.Styling;
using Appalachia.UI.Core.Components.Subsets;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Profiling
{
    public class ProfilingSideBarSubwidgetMetadata : SideBarSubwidgetMetadata<ProfilingSideBarSubwidget,
        ProfilingSideBarSubwidgetMetadata>
    {
        #region Fields and Autoproperties

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public FoldoutComponentSetData fps;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public FoldoutComponentSetData memory;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public FoldoutComponentSetData audio;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public VerticalLayoutGroupSubsetData verticalLayoutGroup;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public FoldoutStyleOverride style;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                FoldoutComponentSetData.CreateOrRefresh(ref fps,    this);
                FoldoutComponentSetData.CreateOrRefresh(ref memory, this);
                FoldoutComponentSetData.CreateOrRefresh(ref audio,  this);
                VerticalLayoutGroupSubsetData.CreateOrRefresh(ref verticalLayoutGroup, this);
            }
        }

        protected override void UpdateFunctionalityInternal(ProfilingSideBarSubwidget subwidget)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
                base.UpdateFunctionalityInternal(subwidget);

                var layoutParent = subwidget.canvas.GameObject;

                VerticalLayoutGroupSubsetData.RefreshAndApply(
                    ref verticalLayoutGroup,
                    this,
                    ref subwidget.verticalLayoutGroup,
                    layoutParent,
                    nameof(VerticalLayoutGroup)
                );

                var layoutObject = subwidget.verticalLayoutGroup.gameObject;

                style = WidgetMetadata.foldoutStyle;
                fps.Style = style;
                memory.Style = style;
                audio.Style = style;

                FoldoutComponentSetData.RefreshAndApply(ref fps,    ref subwidget.fps,    layoutObject, "FPS",   this);
                FoldoutComponentSetData.RefreshAndApply(ref memory, ref subwidget.memory, layoutObject, "RAM",   this);
                FoldoutComponentSetData.RefreshAndApply(ref audio,  ref subwidget.audio,  layoutObject, "Audio", this);
            }
        }
    }
}
