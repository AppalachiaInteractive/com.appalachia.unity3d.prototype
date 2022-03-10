using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Core;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Profiling;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Widgets;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Profiling
{
    [CallStaticConstructorInEditor]
    public class ProfilingActivityBarSubwidget : ActivityBarSubwidget<ProfilingActivityBarSubwidget,
        ProfilingActivityBarSubwidgetMetadata>
    {
        static ProfilingActivityBarSubwidget()
        {
            RegisterDependency<SideBarWidget>(i => _sideBarWidget = i);
            RegisterDependency<ProfilingSideBarSubwidget>(i => _profilingSideBarSubwidget = i);
        }

        #region Static Fields and Autoproperties

        private static SideBarWidget _sideBarWidget;
        private static ProfilingSideBarSubwidget _profilingSideBarSubwidget;

        #endregion

        public override void OnClicked()
        {
            using (_PRF_OnClicked.Auto())
            {
            }
        }

        protected override void OnActivate()
        {
            using (_PRF_OnActivate.Auto())
            {
            }
        }

        protected override void OnDeactivate()
        {
            using (_PRF_OnDeactivate.Auto())
            {
                
            }
        }
    }
}
