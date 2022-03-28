using System.Linq;
using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.UI.Functionality.Images.Groups.Default;
using Appalachia.Utility.Async;
using Appalachia.Utility.Colors;
using Appalachia.Utility.DataStructures.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.GameArea.Widgets
{
    public sealed class GameAreaWidgetMetadata : DeveloperInterfaceMetadata_V01.WidgetMetadata<GameAreaWidget,
        GameAreaWidgetMetadata, GameAreaFeature, GameAreaFeatureMetadata>
    {
        #region Fields and Autoproperties

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        public bool maintainAspectRatio;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public Color color;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        public Sprite logo;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public Color logoColor;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(.1f, 1f)]
        public float logoSize;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {

                initializer.Do(this, nameof(color),               color == Color.clear, () => color = Colors.FromHexCode("1E1E1E"));
                initializer.Do(this, nameof(maintainAspectRatio), () => maintainAspectRatio = true);
                initializer.Do(this, nameof(logoSize),            () => logoSize = .8f);
                initializer.Do(this, nameof(logoColor),           logoColor == Color.clear, () => logoColor = Colors.FromHexCode("1E1E1E"));
            }
        }

        protected override void OnApply(GameAreaWidget widget)
        {
            using (_PRF_OnApply.Auto())
            {
                background.IsElected = true;

                var logoDiff = 1.0f - logoSize;
                var halfDiff = logoDiff * .5f;
                var anchorMin = Vector2.one * halfDiff;
                var anchorMax = Vector2.one * (1.0f - halfDiff);
                
                background.Value.SolidColorBehindImage(color, logo, logoColor, anchorMin, anchorMax);
                
                var firstBackground = background.Value.ConfigList.FirstOrDefault();
                var secondBackground = background.Value.ConfigList.SecondOrDefault();

                if (firstBackground == null)
                {
                    firstBackground = ImageComponentGroupConfig.CreateWithOwner(this);
                    background.Value.ConfigList.Add(firstBackground);
                }

                if (secondBackground == null)
                {
                    secondBackground = ImageComponentGroupConfig.CreateWithOwner(this);
                    background.Value.ConfigList.Add(secondBackground);
                }

                firstBackground.image.color.OverrideValue(color);
                secondBackground.image.sprite.OverrideValue(logo);

                base.OnApply(widget);
            }
        }
    }
}
