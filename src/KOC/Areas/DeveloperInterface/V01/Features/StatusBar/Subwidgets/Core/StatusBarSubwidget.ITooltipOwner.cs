using Appalachia.UI.Functionality.Buttons.Components;
using Appalachia.UI.Functionality.Tooltips.Contracts;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Core
{
    public abstract partial class StatusBarSubwidget<TSubwidget, TSubwidgetMetadata> : ITooltipOwner
    {
        #region ITooltipOwner Members

        public AppaButton TooltipTargetOwner => button.GetTooltipTargetOwner();

        public abstract string GetTooltipContent();

        protected static readonly ProfilerMarker _PRF_GetTooltipContent =
            new ProfilerMarker(_PRF_PFX + nameof(GetTooltipContent));

        #endregion
    }
}
