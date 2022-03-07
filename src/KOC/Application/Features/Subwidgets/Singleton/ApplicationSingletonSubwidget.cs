using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Prototype.KOC.Application.Functionality.Contracts;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;
using Appalachia.UI.Controls.Extensions;
using Appalachia.UI.Core.Styling;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton
{
    [CallStaticConstructorInEditor]
    public abstract class ApplicationSingletonSubwidget<TSubwidget, TISubwidget, TSubwidgetMetadata,
                                                        TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature,
                                                        TFeatureMetadata, TFunctionalitySet, TIService, TIWidget,
                                                        TManager> :
        ApplicationFunctionality<TSubwidget, TSubwidgetMetadata, TManager>,
        ISingletonSubwidget<TISubwidget, TISubwidgetMetadata>,
        IClickable
        where TSubwidget : ApplicationSingletonSubwidget<TSubwidget, TISubwidget, TSubwidgetMetadata,
            TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService,
            TIWidget, TManager>, TISubwidget
        where TISubwidget : class, ISingletonSubwidget<TISubwidget, TISubwidgetMetadata>
        where TSubwidgetMetadata : ApplicationSingletonSubwidgetMetadata<TSubwidget, TISubwidget, TSubwidgetMetadata,
            TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService,
            TIWidget, TManager>, TISubwidgetMetadata
        where TISubwidgetMetadata : class, ISingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
        where TWidget : ApplicationWidgetWithSingletonSubwidgets<TISubwidget, TISubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>, TIWidget
        where TWidgetMetadata : ApplicationWidgetWithSingletonSubwidgetsMetadata<TISubwidget, TISubwidgetMetadata,
            TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>
        where TFeature : ApplicationFeature<TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget,
            TManager>
        where TFeatureMetadata : ApplicationFeatureMetadata<TFeature, TFeatureMetadata, TFunctionalitySet, TIService,
            TIWidget, TManager>
        where TFunctionalitySet : FeatureFunctionalitySet<TIService, TIWidget>, new()
        where TIService : IApplicationService
        where TIWidget : IApplicationWidget
        where TManager : SingletonAppalachiaBehaviour<TManager>, ISingleton<TManager>, IApplicationFunctionalityManager
    {
        static ApplicationSingletonSubwidget()
        {
            var callbacks = RegisterInstanceCallbacks
               .For<ApplicationSingletonSubwidget<TSubwidget, TISubwidget, TSubwidgetMetadata, TISubwidgetMetadata,
                    TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget,
                    TManager>>();

            callbacks.When.Behaviour<TFeature>().IsAvailableThen(i => _feature = i);
            callbacks.When.Behaviour<TWidget>().IsAvailableThen(i => _widget = i);

            RegisterDependency<StyleElementDefaultLookup>(i => _styleElementDefaultLookup = i);
        }

        #region Static Fields and Autoproperties

        private static StyleElementDefaultLookup _styleElementDefaultLookup;

        private static TFeature _feature;

        private static TWidget _widget;

        #endregion

        public virtual bool ResetTransform => false;

        public override bool GuaranteedEventRouting => true;

        public TFeature Feature => _feature;

        public TWidget Widget => _widget;

        protected abstract void OnUpdateSubwidget();

        #region IClickable Members

        public abstract void OnClicked();

        #endregion

        #region ISingletonSubwidget<TISubwidget,TISubwidgetMetadata> Members

        TISubwidgetMetadata ISingletonSubwidget<TISubwidget, TISubwidgetMetadata>.Metadata => Metadata;

        [Button]
        public void UpdateSubwidget()
        {
            using (_PRF_UpdateSubwidget.Auto())
            {
                if (ResetTransform)
                {
                    RectTransform.Reset(RectResetOptions.All);
                }

                OnUpdateSubwidget();
            }
        }

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_OnClicked = new ProfilerMarker(_PRF_PFX + nameof(OnClicked));

        protected static readonly ProfilerMarker _PRF_OnUpdateSubwidget =
            new ProfilerMarker(_PRF_PFX + nameof(OnUpdateSubwidget));

        private static readonly ProfilerMarker _PRF_UpdateSubwidget =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateSubwidget));

        #endregion
    }
}
