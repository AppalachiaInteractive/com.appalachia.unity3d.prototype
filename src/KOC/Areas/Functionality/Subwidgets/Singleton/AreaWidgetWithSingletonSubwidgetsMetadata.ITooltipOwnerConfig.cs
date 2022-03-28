using Appalachia.Core.Objects.Initialization;
using Appalachia.UI.Functionality.Tooltips.Contracts;
using Appalachia.UI.Functionality.Tooltips.Styling;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton
{
    public abstract partial class AreaWidgetWithSingletonSubwidgetsMetadata<TISubwidget, TISubwidgetMetadata, TWidget,
                                                                            TWidgetMetadata, TFeature, TFeatureMetadata,
                                                                            TAreaManager,
                                                                            TAreaMetadata> : ITooltipOwnerConfig
    {
        #region Fields and Autoproperties

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        private TooltipStyleTypes _tooltipStyle;

        #endregion

        private void InitializeTooltipOwnership(Initializer initializer)
        {
            using (_PRF_InitializeTooltipOwnership.Auto())
            {
                initializer.Do(this, nameof(_tooltipStyle), () => { _tooltipStyle = TooltipStyleTypes.Developer; });
            }
        }

        #region ITooltipOwnerConfig Members

        public TooltipStyleTypes TooltipStyle => _tooltipStyle;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_InitializeTooltipOwnership =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeTooltipOwnership));

        #endregion
    }
}
