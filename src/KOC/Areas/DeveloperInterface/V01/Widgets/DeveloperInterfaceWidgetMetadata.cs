using Appalachia.CI.Constants;
using Appalachia.Prototype.KOC.Areas.Common.Widgets;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets
{
    public abstract class
        DeveloperInterfaceWidgetMetadata<TWidget, TWidgetMetadata, TAreaManager, TAreaMetadata> :
            AreaWidgetMetadata<TWidget, TWidgetMetadata, TAreaManager, TAreaMetadata>
        where TWidget : DeveloperInterfaceWidget<TWidget, TWidgetMetadata, TAreaManager, TAreaMetadata>
        where TWidgetMetadata : DeveloperInterfaceWidgetMetadata<TWidget, TWidgetMetadata, TAreaManager,
            TAreaMetadata>
        where TAreaManager : DeveloperInterfaceManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : DeveloperInterfaceMetadata<TAreaManager, TAreaMetadata>
    {
        #region Fields and Autoproperties

        [FoldoutGroup(APPASTR.GroupNames.Common + "/" + APPASTR.GroupNames.General)]
        [OnValueChanged(nameof(InvokeSettingsChanged))]
        public bool inUnscaledView;

        #endregion
    }
}
