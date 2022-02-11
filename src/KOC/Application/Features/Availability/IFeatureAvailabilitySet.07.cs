using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services;
using Appalachia.Prototype.KOC.Application.Features.Widgets;

namespace Appalachia.Prototype.KOC.Application.Features.Availability
{
    public interface
        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7> : IAvailabilitySet<T1, T2, T3, T4, T5, T6, T7>
    {
        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, TNext> AndFeature<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationFeature;

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, TNext> AndFeature<TNext>(TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationFeature;

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, TNext> AndFeatureMetadata<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationFeatureMetadata;

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, TNext> AndFeatureMetadata<TNext>(TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationFeatureMetadata;

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, TNext> AndService<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationService;

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, TNext> AndService<TNext>(TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationService;

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, TNext> AndServiceMetadata<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationServiceMetadata;

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, TNext> AndServiceMetadata<TNext>(TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationServiceMetadata;

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, TNext> AndWidget<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationWidget;

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, TNext> AndWidget<TNext>(TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationWidget;

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, TNext> AndWidgetMetadata<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationWidgetMetadata;

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, TNext> AndWidgetMetadata<TNext>(TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationWidgetMetadata;
    }
}
