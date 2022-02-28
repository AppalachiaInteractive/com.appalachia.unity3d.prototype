using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Core.Preferences;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;
using Appalachia.Utility.Colors;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton
{
    [CallStaticConstructorInEditor]
    public abstract class ApplicationSingletonSubwidget<TSubwidget, TISubwidget, TSubwidgetMetadata,
                                                        TISubwidgetMetadata, TWidget, TWidgetMetadata,
                                                        TFeature, TFeatureMetadata, TFunctionalitySet,
                                                        TIService, TIWidget, TManager> :
        SingletonAppalachiaBehaviour<TSubwidget>,
        ISingletonSubwidget<TISubwidget, TISubwidgetMetadata>
        where TSubwidget : ApplicationSingletonSubwidget<TSubwidget, TISubwidget, TSubwidgetMetadata,
            TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet,
            TIService, TIWidget, TManager>, TISubwidget
        where TISubwidget : class, ISingletonSubwidget<TISubwidget, TISubwidgetMetadata>
        where TSubwidgetMetadata : ApplicationSingletonSubwidgetMetadata<TSubwidget, TISubwidget,
            TSubwidgetMetadata, TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
            TFunctionalitySet, TIService, TIWidget, TManager>, TISubwidgetMetadata
        where TISubwidgetMetadata : class, ISingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
        where TWidget : ApplicationWidgetWithSingletonSubwidgets<TISubwidget, TISubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>,
        TIWidget
        where TWidgetMetadata : ApplicationWidgetWithSingletonSubwidgetsMetadata<TISubwidget,
            TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet,
            TIService, TIWidget, TManager>
        where TFeature : ApplicationFeature<TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget
            , TManager>
        where TFeatureMetadata : ApplicationFeatureMetadata<TFeature, TFeatureMetadata, TFunctionalitySet,
            TIService, TIWidget, TManager>
        where TFunctionalitySet : FeatureFunctionalitySet<TIService, TIWidget>, new()
        where TIService : IApplicationService
        where TIWidget : IApplicationWidget
        where TManager : SingletonAppalachiaBehaviour<TManager>, ISingleton<TManager>,
        IApplicationFunctionalityManager
    {
        static ApplicationSingletonSubwidget()
        {
            var callbacks = RegisterInstanceCallbacks
               .For<ApplicationSingletonSubwidget<TSubwidget, TISubwidget, TSubwidgetMetadata,
                    TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
                    TFunctionalitySet, TIService, TIWidget, TManager>>();

            RegisterDependency<TSubwidgetMetadata>(i => _metadata = i);
            callbacks.When.Behaviour<TFeature>().IsAvailableThen(i => _feature = i);
            callbacks.When.Behaviour<TWidget>().IsAvailableThen(i => _widget = i);
        }

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

        #region Static Fields and Autoproperties

        [PropertyOrder(-10)]
        [ShowInInspector, InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        protected static TSubwidgetMetadata _metadata;

        private static TFeature _feature;

        private static TWidget _widget;

        #endregion

        public override bool GuaranteedEventRouting => true;

        public TFeature Feature => _feature;

        public TSubwidgetMetadata Metadata => _metadata;

        public TWidget Widget => _widget;

        protected Color DisableColor => _disableColor;
        protected Color EnableColor => _enableColor;
        protected Color FunctionalityColor => _functionalityColor;
        protected Color MetadataColor => _metadataColor;
        protected Color NavigationColor => _navigationColor;

        protected abstract void OnUpdateSubwidget();

        #region ISingletonSubwidget<TISubwidget,TISubwidgetMetadata> Members

        TISubwidgetMetadata ISingletonSubwidget<TISubwidget, TISubwidgetMetadata>.Metadata => Metadata;

        [Button]
        public void UpdateSubwidget()
        {
            using (_PRF_UpdateSubwidget.Auto())
            {
                OnUpdateSubwidget();
            }
        }

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_OnUpdateSubwidget =
            new ProfilerMarker(_PRF_PFX + nameof(OnUpdateSubwidget));

        private static readonly ProfilerMarker _PRF_UpdateSubwidget =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateSubwidget));

        #endregion
    }
}
