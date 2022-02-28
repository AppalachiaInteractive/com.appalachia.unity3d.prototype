using Appalachia.Core.Objects.Availability;
using Appalachia.Prototype.KOC.Application.Features.Availability.Contracts;
using Appalachia.Prototype.KOC.Application.Lifetime;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Features.Availability.Extensions
{
    public static class AvailabilitySetExtensions
    {
        public static IFeatureAvailabilitySet WithFeatures(this IAvailabilitySet set)
        {
            var castSet = set as AvailabilitySet;
            return new FeatureAvailabilitySet(null, castSet.SortOrder);
        }

        
        public static IAvailabilitySet<LifetimeComponentManager> LifetimeComponentManager(this IAvailabilitySet set)
        {
            return set.Behaviour<LifetimeComponentManager>();
        }

        internal static FeatureAvailabilitySet<T1> ToFeatureAvailabilitySet<T1>(this AvailabilitySet<T1> set)
        {
            var result = new FeatureAvailabilitySet<T1>(set.SortOrder);
            set.CopyTo(result);
            return result;
        }

        internal static FeatureAvailabilitySet<T1, T2> ToFeatureAvailabilitySet<T1, T2>(
            this AvailabilitySet<T1, T2> set)
        {
            var result = new FeatureAvailabilitySet<T1, T2>(set.SortOrder);
            set.CopyTo(result);
            return result;
        }

        internal static FeatureAvailabilitySet<T1, T2, T3> ToFeatureAvailabilitySet<T1, T2, T3>(
            this AvailabilitySet<T1, T2, T3> set)
        {
            var result = new FeatureAvailabilitySet<T1, T2, T3>(set.SortOrder);
            set.CopyTo(result);
            return result;
        }

        internal static FeatureAvailabilitySet<T1, T2, T3, T4> ToFeatureAvailabilitySet<T1, T2, T3, T4>(
            this AvailabilitySet<T1, T2, T3, T4> set)
        {
            var result = new FeatureAvailabilitySet<T1, T2, T3, T4>(set.SortOrder);
            set.CopyTo(result);
            return result;
        }

        internal static FeatureAvailabilitySet<T1, T2, T3, T4, T5>
            ToFeatureAvailabilitySet<T1, T2, T3, T4, T5>(this AvailabilitySet<T1, T2, T3, T4, T5> set)
        {
            var result = new FeatureAvailabilitySet<T1, T2, T3, T4, T5>(set.SortOrder);
            set.CopyTo(result);
            return result;
        }

        internal static FeatureAvailabilitySet<T1, T2, T3, T4, T5, T6>
            ToFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6>(this AvailabilitySet<T1, T2, T3, T4, T5, T6> set)
        {
            var result = new FeatureAvailabilitySet<T1, T2, T3, T4, T5, T6>(set.SortOrder);
            set.CopyTo(result);
            return result;
        }

        internal static FeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7>
            ToFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7>(
                this AvailabilitySet<T1, T2, T3, T4, T5, T6, T7> set)
        {
            var result = new FeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7>(set.SortOrder);
            set.CopyTo(result);
            return result;
        }

        internal static FeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8>
            ToFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8>(
                this AvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8> set)
        {
            var result = new FeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8>(set.SortOrder);
            set.CopyTo(result);
            return result;
        }

        internal static FeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9>
            ToFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
                this AvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9> set)
        {
            var result = new FeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9>(set.SortOrder);
            set.CopyTo(result);
            return result;
        }

        internal static FeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
            ToFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
                this AvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> set)
        {
            var result = new FeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(set.SortOrder);
            set.CopyTo(result);
            return result;
        }
    }
}
