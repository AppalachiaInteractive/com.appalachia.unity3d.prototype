using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Contracts;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Instanced.Contracts
{
    public interface IApplicationInstancedSubwidget : IApplicationSubwidget
    {
    }

    public interface IApplicationInstancedSubwidget<T> : IApplicationInstancedSubwidget, IApplicationSubwidget<T>
        where T : class, IApplicationInstancedSubwidget<T>
    {
    }

    public interface IApplicationInstancedSubwidget<T, TMetadata> : IApplicationInstancedSubwidget<T>,
                                                                    IApplicationSubwidget<T, TMetadata>,
                                                                    IEnableNotifier
        where T : class, IApplicationInstancedSubwidget<T, TMetadata>
        where TMetadata : class, IApplicationInstancedSubwidgetMetadata<T>
    {
    }
}
