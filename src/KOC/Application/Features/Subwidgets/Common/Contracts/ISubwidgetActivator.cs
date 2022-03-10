using Appalachia.Prototype.KOC.Application.Features.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Contracts;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Common.Contracts
{
    public interface ISubwidgetActivator<T, TM>
        where T : class, IApplicationSubwidget<T, TM>, IActivable
        where TM : class, IApplicationSubwidgetMetadata<T, TM>
    {
        T ActiveSubwidget { get; }
        void DeactivateActiveSubwidget();
        void SetActiveSubwidget(T subwidget);
    }
}
