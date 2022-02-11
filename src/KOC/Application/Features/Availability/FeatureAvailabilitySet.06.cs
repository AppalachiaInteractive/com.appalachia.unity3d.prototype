using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services;
using Appalachia.Prototype.KOC.Application.Features.Widgets;

namespace Appalachia.Prototype.KOC.Application.Features.Availability
{
    public class FeatureAvailabilitySet<T1, T2, T3, T4, T5, T6> : AvailabilitySet<T1, T2, T3, T4, T5, T6>,
                                                                  IFeatureAvailabilitySet<T1, T2, T3, T4, T5,
                                                                      T6>
    {
        public FeatureAvailabilitySet(int sortOrder) : base(sortOrder)
        {
        }

        public IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, TNext> AndFeature<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, TNext> AndService<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, TNext> AndWidget<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        #region IFeatureAvailabilitySet<T1,T2,T3,T4,T5,T6> Members

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, TNext> IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6>
            .AndFeature<TNext>()
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, TNext> AndFeature<TNext>(TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationFeature
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, TNext> AndFeatureMetadata<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationFeatureMetadata
        {
            var result = AndObject<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, TNext> AndFeatureMetadata<TNext>(TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationFeatureMetadata
        {
            var result = AndObject<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, TNext> IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6>
            .AndService<TNext>()
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, TNext> AndService<TNext>(TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationService
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, TNext> AndServiceMetadata<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationServiceMetadata
        {
            var result = AndObject<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, TNext> AndServiceMetadata<TNext>(TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationServiceMetadata
        {
            var result = AndObject<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, TNext> IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6>
            .AndWidget<TNext>()
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, TNext> AndWidget<TNext>(TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationWidget
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, TNext> AndWidgetMetadata<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationWidgetMetadata
        {
            var result = AndObject<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, TNext> AndWidgetMetadata<TNext>(TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationWidgetMetadata
        {
            var result = AndObject<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        #endregion
    }
}
