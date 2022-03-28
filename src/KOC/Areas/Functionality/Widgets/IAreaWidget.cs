using System;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Widgets
{
    public interface IAreaWidget : IApplicationWidget, IAreaFunctionality
    {
        void ForEachSubwidget(Action<IAreaSubwidget> forEachAction);
    }
}
