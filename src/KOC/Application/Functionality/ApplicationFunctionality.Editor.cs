#if UNITY_EDITOR
using Appalachia.CI.Constants;
using Appalachia.Core.Attributes;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Preferences;
using Appalachia.Utility.Colors;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Functionality
{
    [CallStaticConstructorInEditor]
    [SmartLabelChildren]
    [ExecuteAlways]
    public abstract partial class ApplicationFunctionality<TFunctionality, TFunctionalityMetadata, TManager>
    {
        #region Constants and Static Readonly

        protected const string FEATURE_BUTTON_GROUP = nameof(ApplyMetadataButton);

        #endregion

        #region Preferences

        private PREF<Color> _disableColor = PREFS.REG(
            PKG.Prefs.Group,
            "Disabled Color",
            Colors.CadmiumOrange
        );

        private PREF<Color> _enableColor = PREFS.REG(PKG.Prefs.Group, "Enabled Color", Colors.PaleGreen4);

        private PREF<Color> _functionalityColor = PREFS.REG(
            PKG.Prefs.Group,
            "Functionality Color",
            Colors.Teal
        );

        private PREF<Color> _metadataColor = PREFS.REG(PKG.Prefs.Group, "Metadata Color", Colors.PaleGreen4);

        private PREF<Color> _navigationColor = PREFS.REG(PKG.Prefs.Group, "Navigation Color", Colors.SkyBlue);

        #endregion

        protected Color DisableColor => _disableColor;
        protected Color EnableColor => _enableColor;
        protected Color FunctionalityColor => _functionalityColor;
        protected Color MetadataColor => _metadataColor;
        protected Color NavigationColor => _navigationColor;

        [ButtonGroup(FEATURE_BUTTON_GROUP)]
        [LabelText(APPASTR.Apply_Metadata)]
        [GUIColor(nameof(MetadataColor))]
        private void ApplyMetadataButton()
        {
            using (_PRF_ApplyMetadataButton.Auto())
            {
                ApplyMetadata();
            }
        }

        #region Profiling

        protected static readonly ProfilerMarker _PRF_ApplyMetadataButton =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyMetadataButton));

        #endregion
    }
}

#endif
