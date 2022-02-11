using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Application.Features
{
    public interface IApplicationFeature : IApplicationFunctionality
    {
        AppaTask SetToInitialState();
    }

    public interface IApplicationFeature<T> : IApplicationFeature
        where T : IApplicationFeature<T>
    {
    }

    public interface IApplicationFeature<T, TMetadata> : IApplicationFeature<T>
        where T : IApplicationFeature<T, TMetadata>
        where TMetadata : IApplicationFeatureMetadata<T>
    {
    }
}
