﻿using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Instance;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Settings;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Fps
{
    public class RuntimeGraphFpsManager : RuntimeGraphInstanceManager<RuntimeGraphFpsGraph,
        RuntimeGraphFpsManager, RuntimeGraphFpsMonitor, RuntimeGraphFpsText, RuntimeGraphFpsSettings>
    {
        /// <inheritdoc />
        protected override int BasicBackgroundImageIndex => 2;

        /// <inheritdoc />
        protected override int FullBackgroundImageIndex => 0;

        /// <inheritdoc />
        protected override int TextBackgroundImageIndex => 1;

        /// <inheritdoc />
        protected override RuntimeGraphFpsSettings settings => allSettings.fps;
    }
}
