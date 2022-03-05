using Appalachia.Prototype.KOC.Application.Features.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Controlled;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton.Contracts;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Common.Contracts
{
    public interface ISubwidgetActivator<T, TM>
        where T : class, ISingletonSubwidget<T, TM>, IActivable
        where TM : class, ISingletonSubwidgetMetadata<T, TM>
    {
        void DeactivateActiveSubwidget();
        void SetActiveSubwidget(T subwidget);
        T ActiveSubwidget { get; } 
    }

    public interface ISubwidgetActivator<T>
        where T : class, IApplicationControlledSubwidget, IActivable
    {
        void DeactivateActiveSubwidget();
        void SetActiveSubwidget(T subwidget);
        T ActiveSubwidget { get; }
    }
}
