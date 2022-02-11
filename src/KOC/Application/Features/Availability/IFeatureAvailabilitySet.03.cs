using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services;
using Appalachia.Prototype.KOC.Application.Features.Widgets;

// ReSharper disable UnusedParameter.Global

namespace Appalachia.Prototype.KOC.Application.Features.Availability
{
    public interface IFeatureAvailabilitySet<T1, T2, T3> : IAvailabilitySet<T1, T2, T3>
    {
        IFeatureAvailabilitySet<T1, T2, T3, TNext> AndFeature<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationFeature;

        IFeatureAvailabilitySet<T1, T2, T3, TNext> AndFeature<TNext>(TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationFeature;

        IFeatureAvailabilitySet<T1, T2, T3, TNext> AndFeatureMetadata<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationFeatureMetadata;

        IFeatureAvailabilitySet<T1, T2, T3, TNext> AndFeatureMetadata<TNext>(TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationFeatureMetadata;

        IFeatureAvailabilitySet<T1, T2, T3, TNext> AndService<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationService;

        IFeatureAvailabilitySet<T1, T2, T3, TNext> AndService<TNext>(TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationService;

        IFeatureAvailabilitySet<T1, T2, T3, TNext> AndServiceMetadata<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationServiceMetadata;

        IFeatureAvailabilitySet<T1, T2, T3, TNext> AndServiceMetadata<TNext>(TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationServiceMetadata;

        IFeatureAvailabilitySet<T1, T2, T3, TNext> AndWidget<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationWidget;

        IFeatureAvailabilitySet<T1, T2, T3, TNext> AndWidget<TNext>(TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationWidget;

        IFeatureAvailabilitySet<T1, T2, T3, TNext> AndWidgetMetadata<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationWidgetMetadata;

        IFeatureAvailabilitySet<T1, T2, T3, TNext> AndWidgetMetadata<TNext>(TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationWidgetMetadata;
    }
}
