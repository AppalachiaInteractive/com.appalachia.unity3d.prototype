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
        #region Constants and Static Readonly

        private const string TOOLTIP_TEXT = "Performance and Profiling";

        #endregion

        static ProfilingActivityBarSubwidget()
        {
            RegisterDependency<SideBarWidget>(i => _sideBarWidget = i);
            RegisterDependency<ProfilingSideBarSubwidget>(i => _profilingSideBarSubwidget = i);
        }

        #region Static Fields and Autoproperties

        private static ProfilingSideBarSubwidget _profilingSideBarSubwidget;
        private static SideBarWidget _sideBarWidget;

        #endregion

        public override string GetTooltipContent()
        {
            return TOOLTIP_TEXT;
        }

        public override void OnClicked()
        {
            using (_PRF_OnClicked.Auto())
            {
                _sideBarWidget.DeactivateActiveSubwidget();

                if (ReferenceEquals(_sideBarWidget.ActiveSubwidget, _profilingSideBarSubwidget))
                {
                    _sideBarWidget.Hide();
                }
                else
                {
                    _sideBarWidget.Show();
                    _sideBarWidget.SetActiveSubwidget(_profilingSideBarSubwidget);
                }
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
