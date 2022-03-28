using Appalachia.Prototype.KOC.Application.Features.Contracts;
using Appalachia.UI.Functionality.Buttons.Components;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Core
{
    public abstract partial class ActivityBarSubwidget<TSubwidget, TSubwidgetMetadata> : IClickable
    {
        #region IClickable Members

        public abstract void OnClicked();

        public AppaButton ClickableControl => button.Clickable;

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_OnClicked = new ProfilerMarker(_PRF_PFX + nameof(OnClicked));

        #endregion
    }
}
