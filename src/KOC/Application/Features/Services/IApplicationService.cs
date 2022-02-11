using Appalachia.Prototype.KOC.Application.Functionality;

namespace Appalachia.Prototype.KOC.Application.Features.Services
{
    public interface IApplicationService : IApplicationFunctionality
    {
    }

    public interface IApplicationService<T> : IApplicationService
        where T : IApplicationService<T>
    {
    }

    public interface IApplicationService<T, TMetadata> : IApplicationService<T>
        where T : IApplicationService<T, TMetadata>
        where TMetadata : IApplicationServiceMetadata<T>
    {
    }
}
