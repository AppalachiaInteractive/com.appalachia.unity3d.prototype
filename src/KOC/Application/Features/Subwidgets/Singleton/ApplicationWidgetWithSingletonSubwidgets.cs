using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton
{
    [CallStaticConstructorInEditor]
    public abstract class ApplicationWidgetWithSingletonSubwidgets<
        TISubwidget, TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
        TFunctionalitySet, TIService, TIWidget, TManager> : ApplicationWidget<TWidget, TWidgetMetadata,
        TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>
        where TISubwidget : class, ISingletonSubwidget<TISubwidget, TISubwidgetMetadata>
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
        static ApplicationWidgetWithSingletonSubwidgets()
        {
            var callbacks = RegisterInstanceCallbacks
               .For<ApplicationWidgetWithSingletonSubwidgets<TISubwidget, TISubwidgetMetadata, TWidget,
                    TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget,
                    TManager>>();

            void HandleSubwidgetAvailability(TISubwidget subwidget)
            {
                void RegisterSubwidgetAction(TWidget widget)
                {
                    widget.RegisterSubwidget(subwidget);
                }

                callbacks.When.Behaviour<TWidget>().IsAvailableThen(RegisterSubwidgetAction);
            }

            callbacks.When.Any<TISubwidget>().IsAvailableThen(HandleSubwidgetAvailability);
        }

        #region Fields and Autoproperties

        protected List<TISubwidget> _subwidgets;

        #endregion

        public IReadOnlyList<TISubwidget> Subwidgets => _subwidgets;

        /// <summary>
        ///     Adds the specified element to the widget's collection.
        /// </summary>
        /// <param name="element">The element to add.</param>
        public void RegisterSubwidget(TISubwidget element)
        {
            using (_PRF_RegisterSubwidget.Auto())
            {
                _subwidgets ??= new();

                for (var i = 0; i < _subwidgets.Count; i++)
                {
                    if (_subwidgets[i] == element)
                    {
                        return;
                    }
                }

                _subwidgets.Add(element);

                OnRegisterSubwidget(element);
            }
        }

        /// <summary>
        ///     Implements any feature specific handling of the element when it is registered.
        /// </summary>
        /// <param name="element"></param>
        protected abstract void OnRegisterSubwidget(TISubwidget element);

        #region Profiling

        protected static readonly ProfilerMarker _PRF_OnRegisterSubwidget =
            new ProfilerMarker(_PRF_PFX + nameof(OnRegisterSubwidget));

        private static readonly ProfilerMarker _PRF_RegisterSubwidget =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterSubwidget));

        #endregion
    }
}
