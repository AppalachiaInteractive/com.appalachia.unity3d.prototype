using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Common.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Subwidgets;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Widgets
{
    [CallStaticConstructorInEditor]
    public sealed class DevTooltipsWidget : DeveloperInterfaceManager_V01.WidgetWithControlledSubwidgets<
                                                DevTooltipSubwidget, DevTooltipsWidget, DevTooltipsWidgetMetadata,
                                                DevTooltipsFeature, DevTooltipsFeatureMetadata>,
                                            ISubwidgetActivator<DevTooltipSubwidget>
    {
        #region Fields and Autoproperties

        [ShowInInspector] private DevTooltipSubwidget _activeSubwidget;

        #endregion

        public DevTooltipSubwidget ActiveSubwidget => _activeSubwidget;

        public override GameObject GetSubwidgetParent()
        {
            using (_PRF_GetSubwidgetParent.Auto())
            {
                return canvas.GameObject;
            }
        }

        protected override void OnRegisterSubwidget(DevTooltipSubwidget subwidget)
        {
            using (_PRF_OnRegisterSubwidget.Auto())
            {
            }
        }

        #region ISubwidgetActivator<IActivityBarSubwidget,IActivityBarSubwidgetMetadata> Members

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

        public void SetActiveSubwidget(DevTooltipSubwidget subwidget)
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
