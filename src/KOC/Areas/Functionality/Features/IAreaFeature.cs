using System;
using Appalachia.Prototype.KOC.Application.Features;
using Appalachia.Prototype.KOC.Areas.Functionality.Services;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Features
{
    public interface IAreaFeature : IApplicationFeature, IAreaFunctionality
    {
        void ForEachService(Action<IAreaService> forEachService);
        void ForEachWidget(Action<IAreaWidget> forEachWidget);
    }
}
