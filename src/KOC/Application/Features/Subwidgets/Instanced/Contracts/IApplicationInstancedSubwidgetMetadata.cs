using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Contracts;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Instanced.Contracts
{
    public interface IApplicationInstancedSubwidgetMetadata : IApplicationSubwidgetMetadata
    {
    }

    public interface IApplicationInstancedSubwidgetMetadata<in T> : IApplicationSubwidgetMetadata<T>,
                                                                    IApplicationInstancedSubwidgetMetadata
        where T : class, IApplicationInstancedSubwidget
    {
    }

    public interface
        IApplicationInstancedSubwidgetMetadata<in T, TMetadata> : IApplicationInstancedSubwidgetMetadata<T>,
                                                                  IApplicationSubwidgetMetadata<T, TMetadata>
        where T : class, IApplicationInstancedSubwidget<T, TMetadata>
        where TMetadata : class, IApplicationInstancedSubwidgetMetadata<T, TMetadata>
    {
    }
}
