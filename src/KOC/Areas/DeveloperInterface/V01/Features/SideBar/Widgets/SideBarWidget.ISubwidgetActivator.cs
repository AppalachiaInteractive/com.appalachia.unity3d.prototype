using System;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Common.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Contracts;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Widgets
{
    public sealed partial class SideBarWidget : ISubwidgetActivator<ISideBarSubwidget, ISideBarSubwidgetMetadata>
    {
        #region Fields and Autoproperties

        [ShowInInspector]
        [NonSerialized]
        private ISideBarSubwidget _activeSubwidget;

        #endregion

        #region ISubwidgetActivator<ISideBarSubwidget,ISideBarSubwidgetMetadata> Members

        public ISideBarSubwidget ActiveSubwidget => _activeSubwidget;

        public void DeactivateActiveSubwidget()
        {
            using (_PRF_DeactivateActiveSubwidget.Auto())
            {
                if (_activeSubwidget != null)
                {
                    _activeSubwidget.Deactivate();
                    _activeSubwidget = null;
                }
            }
        }

        public void SetActiveSubwidget(ISideBarSubwidget subwidget)
        {
            using (_PRF_SetActiveSubwidget.Auto())
            {
                if (_activeSubwidget != subwidget)
                {
                    if (_activeSubwidget != null)
                    {
                        _activeSubwidget.Deactivate();
                    }

                    _activeSubwidget = subwidget;
                }
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_DeactivateActiveSubwidget =
            new ProfilerMarker(_PRF_PFX + nameof(DeactivateActiveSubwidget));

        private static readonly ProfilerMarker _PRF_SetActiveSubwidget =
            new ProfilerMarker(_PRF_PFX + nameof(SetActiveSubwidget));

        #endregion
    }
}
