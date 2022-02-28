using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;

// ReSharper disable UnusedParameter.Global

namespace Appalachia.Prototype.KOC.Application.Features.Availability.Contracts
{
    public interface IFeatureAvailabilitySet : IAvailabilitySet
    {
        IFeatureAvailabilitySet<TNext> Feature<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationFeature;

        IFeatureAvailabilitySet<TNext> Feature<TNext>(TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationFeature;

        IFeatureAvailabilitySet<TNext> FeatureMetadata<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationFeatureMetadata;

        IFeatureAvailabilitySet<TNext> FeatureMetadata<TNext>(TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationFeatureMetadata;

        IFeatureAvailabilitySet<TNext> Service<TNext>(TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationService;

        IFeatureAvailabilitySet<TNext> Service<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationService;

        IFeatureAvailabilitySet<TNext> ServiceMetadata<TNext>(TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationServiceMetadata;

        IFeatureAvailabilitySet<TNext> ServiceMetadata<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationServiceMetadata;

        IFeatureAvailabilitySet<TNext> Widget<TNext>(TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationWidget;

        IFeatureAvailabilitySet<TNext> Widget<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationWidget;

        IFeatureAvailabilitySet<TNext> WidgetMetadata<TNext>(TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationWidgetMetadata;

        IFeatureAvailabilitySet<TNext> WidgetMetadata<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationWidgetMetadata;
    }
}
