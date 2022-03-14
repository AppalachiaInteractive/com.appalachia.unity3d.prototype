using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Core;
using Appalachia.UI.Functionality.Foldout.Control.Default;
using Appalachia.UI.Functionality.Foldout.Styling;
using Appalachia.UI.Functionality.Layout.Groups.Vertical;
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
        public FoldoutControlConfig fps;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public FoldoutControlConfig memory;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public FoldoutControlConfig audio;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public VerticalLayoutGroupComponentGroupConfig verticalLayoutGroup;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public FoldoutStyleOverride style;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                FoldoutControlConfig.Refresh(ref fps,    this);
                FoldoutControlConfig.Refresh(ref memory, this);
                FoldoutControlConfig.Refresh(ref audio,  this);
                VerticalLayoutGroupComponentGroupConfig.Refresh(ref verticalLayoutGroup, this);
            }
        }

        protected override void UpdateFunctionalityInternal(ProfilingSideBarSubwidget subwidget)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
                base.UpdateFunctionalityInternal(subwidget);

                var layoutParent = subwidget.canvas.GameObject;

                VerticalLayoutGroupComponentGroupConfig.RefreshAndApply(
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

                FoldoutControlConfig.RefreshAndApply(ref fps,    ref subwidget.fps,    layoutObject, "FPS",   this);
                FoldoutControlConfig.RefreshAndApply(ref memory, ref subwidget.memory, layoutObject, "RAM",   this);
                FoldoutControlConfig.RefreshAndApply(ref audio,  ref subwidget.audio,  layoutObject, "Audio", this);
            }
        }
    }
}
