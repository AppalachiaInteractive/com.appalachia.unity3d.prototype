using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Availability.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Availability.Extensions;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;

namespace Appalachia.Prototype.KOC.Application.Features.Availability
{
    public class FeatureAvailabilitySet<T1, T2> : AvailabilitySet<T1, T2>, IFeatureAvailabilitySet<T1, T2>
    {
        public FeatureAvailabilitySet(int sortOrder) : base(sortOrder)
        {
        }

        public IFeatureAvailabilitySet<T1, T2, TNext> AndFeature<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, TNext> AndService<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, TNext> AndWidget<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        #region IFeatureAvailabilitySet<T1,T2> Members

        IFeatureAvailabilitySet<T1, T2, TNext> IFeatureAvailabilitySet<T1, T2>.AndFeature<TNext>()
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, TNext> AndFeature<TNext>(TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationFeature
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, TNext> AndFeatureMetadata<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationFeatureMetadata
        {
            var result = AndObject<TNext>() as AvailabilitySet<T1, T2, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, TNext> AndFeatureMetadata<TNext>(TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationFeatureMetadata
        {
            var result = AndObject<TNext>() as AvailabilitySet<T1, T2, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        IFeatureAvailabilitySet<T1, T2, TNext> IFeatureAvailabilitySet<T1, T2>.AndService<TNext>()
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, TNext> AndService<TNext>(TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationService
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, TNext> AndServiceMetadata<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationServiceMetadata
        {
            var result = AndObject<TNext>() as AvailabilitySet<T1, T2, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, TNext> AndServiceMetadata<TNext>(TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationServiceMetadata
        {
            var result = AndObject<TNext>() as AvailabilitySet<T1, T2, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        IFeatureAvailabilitySet<T1, T2, TNext> IFeatureAvailabilitySet<T1, T2>.AndWidget<TNext>()
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, TNext> AndWidget<TNext>(TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, ISingleton<TNext>, IApplicationWidget
        {
            var result = AndBehaviour<TNext>() as AvailabilitySet<T1, T2, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, TNext> AndWidgetMetadata<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationWidgetMetadata
        {
            var result = AndObject<TNext>() as AvailabilitySet<T1, T2, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<T1, T2, TNext> AndWidgetMetadata<TNext>(TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationWidgetMetadata
        {
            var result = AndObject<TNext>() as AvailabilitySet<T1, T2, TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        #endregion
    }
}
