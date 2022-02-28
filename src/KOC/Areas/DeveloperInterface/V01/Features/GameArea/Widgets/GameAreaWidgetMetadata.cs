using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.GameArea.Widgets
{
    public sealed class GameAreaWidgetMetadata : DeveloperInterfaceMetadata_V01.WidgetMetadata<GameAreaWidget,
        GameAreaWidgetMetadata, GameAreaFeature, GameAreaFeatureMetadata>
    {
        #region Fields and Autoproperties

        [BoxGroup(APPASTR.GroupNames.Size)]
        [OnValueChanged(nameof(OnChanged))]
        public bool maintainAspectRatio;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(maintainAspectRatio), () => maintainAspectRatio = true);
            }
        }
    }
}
