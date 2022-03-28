using System;
using Appalachia.Prototype.KOC.Areas.Functionality.Features;
using Appalachia.Prototype.KOC.Areas.Functionality.Services;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;
using Appalachia.UI.ControlModel.Controls.Default.Contracts;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas
{
    public abstract partial class AreaManager<TManager, TMetadata>
    {
        public void ForEachControl(Action<IAppaUIControl> forEachAction)
        {
            using (_PRF_ForEachControl.Auto())
            {
                ForEachWidget(widget => { widget.ForEachControl(forEachAction); });
            }
        }

        public void ForEachFeature(Action<IAreaFeature> forEachAction)
        {
            using (_PRF_ForEachFeature.Auto())
            {
                for (var featureIndex = 0; featureIndex < _featureManager.Functionality.Features.Count; featureIndex++)
                {
                    var feature = _featureManager.Functionality.Features[featureIndex];

                    forEachAction(feature);
                }
            }
        }

        public void ForEachService(Action<IAreaService> forEachAction)
        {
            using (_PRF_ForEachService.Auto())
            {
                ForEachFeature(feature => { feature.ForEachService(forEachAction); });
            }
        }

        public void ForEachSubwidget(Action<IAreaSubwidget> forEachAction)
        {
            using (_PRF_ForEachSubwidget.Auto())
            {
                ForEachWidget(widget => { widget.ForEachSubwidget(forEachAction); });
            }
        }

        public void ForEachWidget(Action<IAreaWidget> forEachAction)
        {
            using (_PRF_ForEachWidget.Auto())
            {
                ForEachFeature(feature => { feature.ForEachWidget(forEachAction); });
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_ForEachControl =
            new ProfilerMarker(_PRF_PFX + nameof(ForEachControl));

        private static readonly ProfilerMarker _PRF_ForEachFeature =
            new ProfilerMarker(_PRF_PFX + nameof(ForEachFeature));

        private static readonly ProfilerMarker _PRF_ForEachService =
            new ProfilerMarker(_PRF_PFX + nameof(ForEachService));

        private static readonly ProfilerMarker _PRF_ForEachSubwidget =
            new ProfilerMarker(_PRF_PFX + nameof(ForEachSubwidget));

        private static readonly ProfilerMarker _PRF_ForEachWidget =
            new ProfilerMarker(_PRF_PFX + nameof(ForEachWidget));

        #endregion
    }
}
