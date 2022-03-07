using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Availability.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Availability.Extensions;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;

namespace Appalachia.Prototype.KOC.Application.Features.Availability
{
    public class FeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9> :
        AvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9>,
        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        public FeatureAvailabilitySet(int sortOrder) : base(sortOrder)
        {
        }

        public IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext> AndFeature<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext> AndService<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext> AndWidget<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        #region IFeatureAvailabilitySet<T1,T2,T3,T4,T5,T6,T7,T8,T9> Members

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext>
            IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9>.AndFeature<TNext>()
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext> AndFeature<TNext>(
            TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationFeature
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext> AndFeatureMetadata<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationFeatureMetadata
        {
            var result = AndObject<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext> AndFeatureMetadata<TNext>(
            TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationFeatureMetadata
        {
            var result = AndObject<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext>
            IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9>.AndService<TNext>()
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext> AndService<TNext>(
            TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationService
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext> AndServiceMetadata<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationServiceMetadata
        {
            var result = AndObject<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext> AndServiceMetadata<TNext>(
            TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationServiceMetadata
        {
            var result = AndObject<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext>
            IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9>.AndWidget<TNext>()
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext> AndWidget<TNext>(
            TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationWidget
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext> AndWidgetMetadata<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationWidgetMetadata
        {
            var result = AndObject<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext> AndWidgetMetadata<TNext>(
            TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationWidgetMetadata
        {
            var result = AndObject<TNext>() as AvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        #endregion
    }
}