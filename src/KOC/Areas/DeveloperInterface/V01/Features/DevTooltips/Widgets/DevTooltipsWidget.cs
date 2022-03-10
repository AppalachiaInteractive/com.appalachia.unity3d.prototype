using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Common.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Subwidgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Subwidgets.Contracts;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Widgets
{
    [CallStaticConstructorInEditor]
    public sealed class DevTooltipsWidget : DeveloperInterfaceManager_V01.WidgetWithInstancedSubwidgets<
                                                DevTooltipSubwidget, DevTooltipSubwidgetMetadata, IDevTooltipSubwidget,
                                                IDevTooltipSubwidgetMetadata, DevTooltipsWidget,
                                                DevTooltipsWidgetMetadata, DevTooltipsFeature,
                                                DevTooltipsFeatureMetadata>,
                                            ISubwidgetActivator<IDevTooltipSubwidget, IDevTooltipSubwidgetMetadata>
    {
        #region Fields and Autoproperties

        [ShowInInspector] private IDevTooltipSubwidget _activeSubwidget;

        #endregion

        public override void ValidateSubwidgets()
        {
            using (_PRF_ValidateSubwidgets.Auto())
            {
                EnsureSubwidgetsHaveCorrectParent(_subwidgets, SubwidgetParent.transform);
            }
        }

        protected override void OnRegisterSubwidget(DevTooltipSubwidget subwidget)
        {
            using (_PRF_OnRegisterSubwidget.Auto())
            {
            }
        }

        #region ISubwidgetActivator<IDevTooltipSubwidget,IDevTooltipSubwidgetMetadata> Members

        public IDevTooltipSubwidget ActiveSubwidget => _activeSubwidget;

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

        public void SetActiveSubwidget(IDevTooltipSubwidget subwidget)
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
