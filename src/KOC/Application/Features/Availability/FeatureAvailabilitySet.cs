using System;
using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Availability.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Availability.Extensions;
using Appalachia.Prototype.KOC.Application.Features.Services;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;

// ReSharper disable UnusedParameter.Global

namespace Appalachia.Prototype.KOC.Application.Features.Availability
{
    public class FeatureAvailabilitySet : AvailabilitySet, IFeatureAvailabilitySet
    {
        public FeatureAvailabilitySet(Type owner, int? sortOrder = null) : base(owner, sortOrder)
        {
        }
        
        public IFeatureAvailabilitySet<TNext> Feature<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, IApplicationFeature
        {
            var result = Behaviour<TNext>() as AvailabilitySet<TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<TNext> Feature<TNext>(TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, IApplicationFeature
        {
            var result = Behaviour<TNext>() as AvailabilitySet<TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<TNext> Service<TNext>(TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, IApplicationService
        {
            var result = Behaviour<TNext>() as AvailabilitySet<TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<TNext> Service<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, IApplicationService
        {
            var result = Behaviour<TNext>() as AvailabilitySet<TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<TNext> Widget<TNext>(TNext unused)
            where TNext : SingletonAppalachiaBehaviour<TNext>, IApplicationWidget
        {
            var result = Behaviour<TNext>() as AvailabilitySet<TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<TNext> Widget<TNext>()
            where TNext : SingletonAppalachiaBehaviour<TNext>, IApplicationWidget
        {
            var result = Behaviour<TNext>() as AvailabilitySet<TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        #region IFeatureAvailabilitySet Members

        public IFeatureAvailabilitySet<TNext> FeatureMetadata<TNext>(TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationFeatureMetadata
        {
            var result = Object<TNext>() as AvailabilitySet<TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        IFeatureAvailabilitySet<TNext> IFeatureAvailabilitySet.Service<TNext>(TNext unused)
        {
            var result = Behaviour<TNext>() as AvailabilitySet<TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        IFeatureAvailabilitySet<TNext> IFeatureAvailabilitySet.Service<TNext>()
        {
            var result = Behaviour<TNext>() as AvailabilitySet<TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<TNext> ServiceMetadata<TNext>(TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationServiceMetadata
        {
            var result = Object<TNext>() as AvailabilitySet<TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<TNext> ServiceMetadata<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationServiceMetadata
        {
            var result = Object<TNext>() as AvailabilitySet<TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        IFeatureAvailabilitySet<TNext> IFeatureAvailabilitySet.Widget<TNext>(TNext unused)
        {
            var result = Behaviour<TNext>() as AvailabilitySet<TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        IFeatureAvailabilitySet<TNext> IFeatureAvailabilitySet.Widget<TNext>()
        {
            var result = Behaviour<TNext>() as AvailabilitySet<TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<TNext> WidgetMetadata<TNext>(TNext unused)
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationWidgetMetadata
        {
            var result = Object<TNext>() as AvailabilitySet<TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<TNext> WidgetMetadata<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationWidgetMetadata
        {
            var result = Object<TNext>() as AvailabilitySet<TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        IFeatureAvailabilitySet<TNext> IFeatureAvailabilitySet.Feature<TNext>(TNext unused)
        {
            var result = Behaviour<TNext>() as AvailabilitySet<TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        public IFeatureAvailabilitySet<TNext> FeatureMetadata<TNext>()
            where TNext : SingletonAppalachiaObject<TNext>, ISingleton<TNext>, IApplicationFeatureMetadata
        {
            var result = Object<TNext>() as AvailabilitySet<TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        IFeatureAvailabilitySet<TNext> IFeatureAvailabilitySet.Feature<TNext>()
        {
            var result = Behaviour<TNext>() as AvailabilitySet<TNext>;
            return result.ToFeatureAvailabilitySet();
        }

        #endregion
    }
}
