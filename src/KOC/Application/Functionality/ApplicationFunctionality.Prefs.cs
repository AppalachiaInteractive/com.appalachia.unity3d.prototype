#if UNITY_EDITOR
using System;
using Appalachia.Core.Preferences;
using Appalachia.Utility.Colors;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Functionality
{
    public abstract partial class ApplicationFunctionality<TFunctionality, TFunctionalityMetadata, TManager>
    {
        #region Preferences

        [NonSerialized] private PREF<Color> _disableColor;
        [NonSerialized] private PREF<Color> _enableColor;
        [NonSerialized] private PREF<Color> _functionalityColor;
        [NonSerialized] private PREF<Color> _metadataColor;
        [NonSerialized] private PREF<Color> _navigationColor;

        #endregion

        protected Color DisableColor => _disableColor ?? Color.white;
        protected Color EnableColor => _enableColor ?? Color.white;
        protected Color FunctionalityColor => _functionalityColor ?? Color.white;
        protected Color MetadataColor => _metadataColor ?? Color.white;
        protected Color NavigationColor => _navigationColor ?? Color.white;

        public override void InitializePreferences()
        {
            base.InitializePreferences();

            _disableColor = PREFS.REG(PKG.Prefs.Group, "Disabled Color", Colors.CadmiumOrange);

            _enableColor = PREFS.REG(PKG.Prefs.Group, "Enabled Color", Colors.PaleGreen4);

            _functionalityColor = PREFS.REG(PKG.Prefs.Group, "Functionality Color", Colors.Teal);

            _metadataColor = PREFS.REG(PKG.Prefs.Group, "Metadata Color", Colors.PaleGreen4);

            _navigationColor = PREFS.REG(PKG.Prefs.Group, "Navigation Color", Colors.SkyBlue);
        }
    }
}

#endif
