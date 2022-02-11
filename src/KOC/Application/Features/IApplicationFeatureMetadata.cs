namespace Appalachia.Prototype.KOC.Application.Features
{
    public interface IApplicationFeatureMetadata
    {
    }

    public interface IApplicationFeatureMetadata<T> : IApplicationFeatureMetadata
        where T : IApplicationFeature
    {
    }

    public interface IApplicationFeatureMetadata<T, TMetadata> : IApplicationFeatureMetadata<T>
        where T : IApplicationFeature<T, TMetadata>
        where TMetadata : IApplicationFeatureMetadata<T, TMetadata>
    {
    }
}
