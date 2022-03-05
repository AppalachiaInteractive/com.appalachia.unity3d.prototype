using System.Collections.Generic;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Application.Features
{
    public interface IApplicationFeature : IApplicationFunctionality
    {
        IEnumerable<IApplicationService> Services { get; }
        IEnumerable<IApplicationWidget> Widgets { get; }
        AppaTask SetToInitialState();
        void SortServices();
        void SortWidgets();
    }

    public interface IApplicationFeature<T> : IApplicationFeature
        where T : IApplicationFeature<T>
    {
    }

    public interface IApplicationFeature<T, TMetadata> : IApplicationFeature<T>
        where T : IApplicationFeature<T, TMetadata>
        where TMetadata : IApplicationFeatureMetadata<T>
    {
    }
}
