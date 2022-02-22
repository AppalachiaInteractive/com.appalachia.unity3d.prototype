using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.GameArea.Widgets;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.GameArea
{
    [CallStaticConstructorInEditor]
    public class GameAreaFeature : DeveloperInterfaceManager_V01.Feature<GameAreaFeature,
        GameAreaFeatureMetadata>
    {
        static GameAreaFeature()
        {
            FunctionalitySet.RegisterWidget<GameAreaWidget>(_dependencyTracker, i => _gameAreaWidget = i);
        }

        #region Static Fields and Autoproperties

        private static GameAreaWidget _gameAreaWidget;

        #endregion
    }
}
