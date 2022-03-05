using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Core.Preferences;
using Appalachia.Prototype.KOC.Application.Features.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;
using Appalachia.UI.Controls.Extensions;
using Appalachia.Utility.Colors;
using Appalachia.Utility.Events;
using Appalachia.Utility.Events.Extensions;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Controlled
{
    [CallStaticConstructorInEditor]
    public abstract class ApplicationControlledSubwidget<TSubwidget, TWidget, TWidgetMetadata, TFeature,
                                                         TFeatureMetadata, TFunctionalitySet, TIService, TIWidget,
                                                         TManager> : AppalachiaBehaviour<TSubwidget>,
                                                                     IApplicationControlledSubwidget,
                                                                     IClickable
        where TSubwidget :
        ApplicationControlledSubwidget<TSubwidget, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
            TFunctionalitySet, TIService, TIWidget, TManager>, IEnableNotifier
        where TWidget : ApplicationWidgetWithControlledSubwidgets<TSubwidget, TWidget, TWidgetMetadata, TFeature,
            TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>, TIWidget
        where TWidgetMetadata : ApplicationWidgetWithControlledSubwidgetsMetadata<TSubwidget, TWidget, TWidgetMetadata,
            TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>
        where TFeature : ApplicationFeature<TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget,
            TManager>
        where TFeatureMetadata : ApplicationFeatureMetadata<TFeature, TFeatureMetadata, TFunctionalitySet, TIService,
            TIWidget, TManager>
        where TFunctionalitySet : FeatureFunctionalitySet<TIService, TIWidget>, new()
        where TIService : IApplicationService
        where TIWidget : IApplicationWidget
        where TManager : SingletonAppalachiaBehaviour<TManager>, ISingleton<TManager>, IApplicationFunctionalityManager
    {
        #region Constants and Static Readonly

        private const string NAME_FORMAT_STRING = "{0} {1}";

        #endregion

        static ApplicationControlledSubwidget()
        {
            var callbacks = RegisterInstanceCallbacks
               .For<ApplicationControlledSubwidget<TSubwidget, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
                    TFunctionalitySet, TIService, TIWidget, TManager>>();

            callbacks.When.Behaviour<TFeature>().IsAvailableThen(i => _feature = i);
            callbacks.When.Behaviour<TWidget>().IsAvailableThen(i => _widget = i);
            callbacks.When.Object<TWidgetMetadata>().IsAvailableThen(i => _metadata = i);
        }

        #region Preferences

        private PREF<Color> _disableColor = PREFS.REG(PKG.Prefs.Group, "Disabled Color", Colors.CadmiumOrange);

        private PREF<Color> _enableColor = PREFS.REG(PKG.Prefs.Group, "Enabled Color", Colors.PaleGreen4);

        private PREF<Color> _functionalityColor = PREFS.REG(PKG.Prefs.Group, "Functionality Color", Colors.Teal);

        private PREF<Color> _metadataColor = PREFS.REG(PKG.Prefs.Group, "Metadata Color", Colors.PaleGreen4);

        private PREF<Color> _navigationColor = PREFS.REG(PKG.Prefs.Group, "Navigation Color", Colors.SkyBlue);

        #endregion

        #region Static Fields and Autoproperties

        private static TFeature _feature;
        private static TWidget _widget;

        [ShowInInspector] private static TWidgetMetadata _metadata;

        #endregion

        #region Fields and Autoproperties

        private AppaEvent.Data UpdateRequested;

        [SerializeField] private string _subwidgetNamePrefix;

        #endregion

        public override bool GuaranteedEventRouting => true;

        public string SubwidgetName => GetSubwidgetName(_subwidgetNamePrefix);

        public TFeature Feature => _feature;

        public TWidget Widget => _widget;

        public TWidgetMetadata Metadata => _metadata;

        protected Color DisableColor => _disableColor;
        protected Color EnableColor => _enableColor;
        protected Color FunctionalityColor => _functionalityColor;
        protected Color MetadataColor => _metadataColor;
        protected Color NavigationColor => _navigationColor;

        public static void RefreshAndUpdateSubwidget(ref TSubwidget subwidget, string subwidgetNamePrefix)
        {
            using (_PRF_RefreshAndUpdateSubwidget.Auto())
            {
                var parentObject = _widget.GetSubwidgetParent();

                GameObject subwidgetObject = null;
                var subwidgetName = GetSubwidgetName(subwidgetNamePrefix);

                if (subwidget == null)
                {
                    parentObject.GetOrAddChild(ref subwidgetObject, subwidgetName, true);

                    subwidgetObject.GetOrAddComponent(ref subwidget);
                }
                else
                {
                    subwidgetObject = subwidget.gameObject;
                }

                subwidget._subwidgetNamePrefix = subwidgetNamePrefix;

                subwidgetObject.SetParentTo(parentObject);

                subwidget.UpdateSubwidget();
            }
        }

        protected abstract void OnUpdateSubwidget();

        private static string GetSubwidgetName(string namePrefix)
        {
            using (_PRF_GetSubwidgetName.Auto())
            {
                return ZString.Format(NAME_FORMAT_STRING, namePrefix, typeof(TSubwidget).Name);
            }
        }

        #region IApplicationControlledSubwidget Members

        public void RequestUpdate()
        {
            using (_PRF_RequestUpdate.Auto())
            {
                UpdateRequested.RaiseEvent();
            }
        }

        public void SubscribeToUpdateRequests(AppaEvent.Handler onUpdateRequested)
        {
            using (_PRF_SubscribeToUpdateRequests.Auto())
            {
                UpdateRequested.Event += onUpdateRequested;
            }
        }

        [Button]
        public void UpdateSubwidget()
        {
            using (_PRF_UpdateSubwidget.Auto())
            {
                name = SubwidgetName;

                RectTransform.FullScreen(true);
                RequestUpdate();
                OnUpdateSubwidget();
            }
        }

        #endregion

        #region IClickable Members

        public abstract void OnClicked();

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_OnUpdateSubwidget =
            new ProfilerMarker(_PRF_PFX + nameof(OnUpdateSubwidget));

        private static readonly ProfilerMarker _PRF_GetSubwidgetName =
            new ProfilerMarker(_PRF_PFX + nameof(GetSubwidgetName));

        private static readonly ProfilerMarker _PRF_RefreshAndUpdateSubwidget =
            new ProfilerMarker(_PRF_PFX + nameof(RefreshAndUpdateSubwidget));

        protected static readonly ProfilerMarker _PRF_OnClicked = new ProfilerMarker(_PRF_PFX + nameof(OnClicked));

        private static readonly ProfilerMarker _PRF_RequestUpdate =
            new ProfilerMarker(_PRF_PFX + nameof(RequestUpdate));

        private static readonly ProfilerMarker _PRF_SubscribeToUpdateRequests =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeToUpdateRequests));

        private static readonly ProfilerMarker _PRF_UpdateSubwidget =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateSubwidget));

        #endregion
    }
}
