using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Features;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Tooltips.Widgets;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Tooltips
{
    [CallStaticConstructorInEditor]
    public class TooltipsFeature : LifetimeFeature<TooltipsFeature, TooltipsFeatureMetadata>
    {
        static TooltipsFeature()
        {
            FunctionalitySet.RegisterWidget<TooltipsWidget>(_dependencyTracker, i => _tooltipsWidget = i);
        }

        #region Static Fields and Autoproperties

        private static TooltipsWidget _tooltipsWidget;

        #endregion
    }
}
