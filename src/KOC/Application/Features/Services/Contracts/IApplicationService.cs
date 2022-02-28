using Appalachia.Prototype.KOC.Application.Functionality;

namespace Appalachia.Prototype.KOC.Application.Features.Services.Contracts
{
    public interface IApplicationService : IApplicationFunctionality
    {
        void DisableService();
        void EnableService();
        void DisableFeature();
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
