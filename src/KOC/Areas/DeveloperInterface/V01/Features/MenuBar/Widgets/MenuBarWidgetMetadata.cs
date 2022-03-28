using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Appalachia.Utility.Colors;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.MenuBar.Widgets
{
    public sealed class MenuBarWidgetMetadata : DeveloperInterfaceMetadata_V01.WidgetMetadata<MenuBarWidget,
        MenuBarWidgetMetadata, MenuBarFeature, MenuBarFeatureMetadata>
    {
        #region Fields and Autoproperties

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0.015f, 0.045f)]
        public float height;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public Color color;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        public bool maintainAspectRatio;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        public Sprite logo;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public Color logoColor;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(12f, 36f)]
        public float logoSize;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            initializer.Do(this, nameof(height), () => height = 0.03f);
            initializer.Do(this, nameof(color), color == Color.clear, () => color = Colors.FromHexCode("323233"));
            initializer.Do(this, nameof(maintainAspectRatio), () => maintainAspectRatio = true);
            initializer.Do(this, nameof(logoSize), () => logoSize = 24f);
            initializer.Do(
                this,
                nameof(logoColor),
                logoColor == Color.clear,
                () => logoColor = Colors.FromHexCode("#FFFFFF")
            );
        }

        /// <inheritdoc />
        protected override void OnApply(MenuBarWidget widget)
        {
            using (_PRF_OnApply.Auto())
            {
                background.IsElected = true;

                var anchorMin = new Vector2(0f, 0f);
                var anchorMax = new Vector2(0f, 1f);
                background.Value.SolidColorBehindImage(
                    color,
                    logo,
                    logoColor,
                    anchorMin,
                    anchorMax,
                    logoSize,
                    0f,
                    logoSize,
                    logoSize
                );

                base.OnApply(widget);
            }
        }
    }
}
