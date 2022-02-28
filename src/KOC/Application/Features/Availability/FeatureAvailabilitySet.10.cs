using Appalachia.Core.Objects.Availability;
using Appalachia.Prototype.KOC.Application.Features.Availability.Contracts;

namespace Appalachia.Prototype.KOC.Application.Features.Availability
{
    public class FeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> :
        AvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>,
        IFeatureAvailabilitySet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    {
        public FeatureAvailabilitySet(int sortOrder) : base(sortOrder)
        {
        }
    }
}
