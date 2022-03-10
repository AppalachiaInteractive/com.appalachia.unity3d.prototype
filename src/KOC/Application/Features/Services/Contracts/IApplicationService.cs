using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Prototype.KOC.Application.Functionality.Contracts;

namespace Appalachia.Prototype.KOC.Application.Features.Services.Contracts
{
    public interface IApplicationService : IApplicationFunctionality
    {
        void DisableService();
        void EnableService();
        void OnDisableFeature();
        void EnableFeature();
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
