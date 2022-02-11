namespace Appalachia.Prototype.KOC.Application.Features.Services
{
    public interface IApplicationServiceMetadata
    {
    }

    public interface IApplicationServiceMetadata<T> : IApplicationServiceMetadata
        where T : IApplicationService
    {
    }

    public interface IApplicationServiceMetadata<T, TMetadata> : IApplicationServiceMetadata<T>
        where T : IApplicationService<T, TMetadata>
        where TMetadata : IApplicationServiceMetadata<T, TMetadata>
    {
    }
}
