using System;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Subwidgets;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Widgets
{
    public interface ILifetimeWidget : IApplicationWidget, ILifetimeFunctionality
    {
        void ForEachSubwidget(Action<ILifetimeSubwidget> forEachAction);
    }
}
