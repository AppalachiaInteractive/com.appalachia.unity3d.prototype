using System;
using Appalachia.CI.Constants;
using Appalachia.Prototype.KOC.Areas.Functionality.Templates.Tooltips.Widgets;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Tooltips.Widgets
{
    [Serializable]
    public sealed class DeveloperInterfaceTooltipsWidgetMetadata : AreaTooltipsWidgetMetadata<
        DeveloperInterfaceTooltipsWidget, DeveloperInterfaceTooltipsWidgetMetadata, DeveloperInterfaceTooltipsFeature,
        DeveloperInterfaceTooltipsFeatureMetadata, DeveloperInterfaceManager_V01, DeveloperInterfaceMetadata_V01>
    {
        #region Fields and Autoproperties

        [FoldoutGroup(APPASTR.GroupNames.Common)]
        [OnValueChanged(nameof(OnChanged))]
        public bool inUnscaledView;

        #endregion
    }
}
