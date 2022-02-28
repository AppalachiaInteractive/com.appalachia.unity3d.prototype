using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;

namespace Appalachia.Prototype.KOC.Application.Features.Availability.Contracts
{
    public interface IFeatureAvailabilitySet<T1, T2, T3, T4, T5> : IAvailabilitySet<T1, T2, T3, T4, T5>
    {
        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, TNext> AndFeature<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationFeature;

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, TNext> AndFeature<TNext>(TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationFeature;

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, TNext> AndFeatureMetadata<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationFeatureMetadata;

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, TNext> AndFeatureMetadata<TNext>(TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationFeatureMetadata;

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, TNext> AndService<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationService;

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, TNext> AndService<TNext>(TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationService;

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, TNext> AndServiceMetadata<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationServiceMetadata;

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, TNext> AndServiceMetadata<TNext>(TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationServiceMetadata;

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, TNext> AndWidget<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationWidget;

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, TNext> AndWidget<TNext>(TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationWidget;

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, TNext> AndWidgetMetadata<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationWidgetMetadata;

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, TNext> AndWidgetMetadata<TNext>(TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationWidgetMetadata;
    }
}
