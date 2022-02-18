#if UNITY_EDITOR
using Appalachia.Core.Preferences;
using Appalachia.Utility.Colors;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.FunctionalitySets
{
    public abstract partial class FeatureManager<T, TFeatureSet, TIFeature>
    {
        #region Preferences

        private PREF<Color> _functionalityColor = PREFS.REG(
            PKG.Prefs.Group,
            "Functionality Color",
            Colors.Teal
        );

        #endregion

        protected Color FunctionalityColor => _functionalityColor;

        [Button]
        [GUIColor(nameof(FunctionalityColor))]
        private void ApplyAllMetadataButton()
        {
            using (_PRF_ApplyAllMetadataButton.Auto())
            {
                ApplyAllMetadata();
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_ApplyAllMetadataButton =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyAllMetadataButton));

        #endregion
    }
}

#endif
