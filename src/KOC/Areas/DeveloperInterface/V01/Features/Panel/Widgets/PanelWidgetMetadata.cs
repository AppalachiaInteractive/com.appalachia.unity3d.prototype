using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Appalachia.Utility.Colors;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Panel.Widgets
{
    public sealed class PanelWidgetMetadata : DeveloperInterfaceMetadata_V01.WidgetMetadata<PanelWidget,
        PanelWidgetMetadata, PanelFeature, PanelFeatureMetadata>
    {
        #region Fields and Autoproperties

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0.25f, 0.35f)]
        public float height;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public Color color;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {

                initializer.Do(this, nameof(height), () => height = 0.3f);
                initializer.Do(this, nameof(color),  color == Color.clear, () => color = Colors.FromHexCode("1E1E1E"));
            }
        }

        /// <inheritdoc />
        protected override void OnApply(PanelWidget widget)
        {
            using (_PRF_OnApply.Auto())
            {
                background.IsElected = true;

                background.Value.SolidColor(color);

                base.OnApply(widget);
            }
        }
    }
}
