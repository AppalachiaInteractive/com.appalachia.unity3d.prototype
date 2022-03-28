using System;
using Appalachia.Prototype.KOC.Application.Features;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Services;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Widgets;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Features
{
    public interface ILifetimeFeature : IApplicationFeature, ILifetimeFunctionality
    {
        void ForEachService(Action<ILifetimeService> forEachAction);
        void ForEachWidget(Action<ILifetimeWidget> forEachAction);
    }
}
