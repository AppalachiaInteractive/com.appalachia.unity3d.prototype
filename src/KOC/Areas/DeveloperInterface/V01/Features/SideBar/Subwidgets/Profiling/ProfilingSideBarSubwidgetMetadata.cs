using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Core;
using Appalachia.UI.Functionality.Foldout.Control.Default;
using Appalachia.UI.Functionality.Foldout.Styling;
using Appalachia.UI.Functionality.Layout.Groups.Vertical;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;

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
        public FoldoutStyleTypes style;

        #endregion

        protected override int DefaultPriority => 10;

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                VerticalLayoutGroupComponentGroupConfig.Refresh(ref verticalLayoutGroup, this);
                FoldoutControlConfig.Refresh(ref fps,    this);
                FoldoutControlConfig.Refresh(ref memory, this);
                FoldoutControlConfig.Refresh(ref audio,  this);
            }
        }

        protected override void BeforeApplying(ProfilingSideBarSubwidget subwidget)
        {
            using (_PRF_BeforeApplying.Auto())
            {
                base.BeforeApplying(subwidget);

                fps.foldoutButton.tooltip.tooltipStyle = TooltipStyle;
                memory.foldoutButton.tooltip.tooltipStyle = TooltipStyle;
                audio.foldoutButton.tooltip.tooltipStyle = TooltipStyle;
            }
        }

        protected override void OnApply(ProfilingSideBarSubwidget subwidget)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(subwidget);

                verticalLayoutGroup.Apply(subwidget.verticalLayoutGroup);

                style = WidgetMetadata.foldoutStyle;
                fps.Style = style;
                memory.Style = style;
                audio.Style = style;

                fps.Apply(subwidget.fps);
                memory.Apply(subwidget.memory);
                audio.Apply(subwidget.audio);
            }
        }
    }
}
