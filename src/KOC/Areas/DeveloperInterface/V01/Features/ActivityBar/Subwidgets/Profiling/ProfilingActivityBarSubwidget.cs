using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Core;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Widgets;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Profiling
{
    [CallStaticConstructorInEditor]
    public class ProfilingActivityBarSubwidget : ActivityBarSubwidget<ProfilingActivityBarSubwidget,
        ProfilingActivityBarSubwidgetMetadata>
    {
        
        static ProfilingActivityBarSubwidget()
        {
            
        }

        private static SideBarWidget _sideBarWidget;
        
        public override void OnClicked()
        {
            using (_PRF_OnClicked.Auto())
            {
            }
        }

        protected override void OnDeactivate()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActivate()
        {
            throw new System.NotImplementedException();
        }
    }
}
