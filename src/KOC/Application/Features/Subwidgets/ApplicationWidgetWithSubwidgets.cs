using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Core.Objects.Routing;
using Appalachia.Prototype.KOC.Application.Features.Services;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;
using Appalachia.Utility.Events;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets
{
    [CallStaticConstructorInEditor]
    public abstract class ApplicationWidgetWithSubwidgets<TSubwidget, TSubwidgetMetadata, TWidget,
                                                          TWidgetMetadata, TFeature, TFeatureMetadata,
                                                          TFunctionalitySet, TIService, TIWidget, TManager> :
        ApplicationWidget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService,
            TIWidget, TManager>
        where TWidget : ApplicationWidgetWithSubwidgets<TSubwidget, TSubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>,
        TIWidget
        where TWidgetMetadata : ApplicationWidgetWithSubwidgetsMetadata<TSubwidget, TSubwidgetMetadata,
            TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget,
            TManager>
        where TSubwidget : ApplicationSubwidget<TSubwidget, TSubwidgetMetadata, TWidget, TWidgetMetadata,
            TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>, IEnableNotifier
        where TSubwidgetMetadata : ApplicationSubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>
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
        static ApplicationWidgetWithSubwidgets()
        {
            void RespondToSubwidgetBeingEnabled(AppaEvent<TSubwidget>.Args subwidget)
            {
                void RegisterSubwidgetWithWidget(TWidget widget)
                {
                    widget.RegisterSubwidget(subwidget);
                }

                RegisterInstanceCallbacks
                   .For<ApplicationWidgetWithSubwidgets<TSubwidget, TSubwidgetMetadata, TWidget,
                        TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget,
                        TManager>>()
                   .When.Behaviour<TWidget>()
                   .IsAvailableThen(RegisterSubwidgetWithWidget);
            }

            ObjectEnableEventRouter.SubscribeTo<TSubwidget>(RespondToSubwidgetBeingEnabled);
        }

        #region Fields and Autoproperties

        protected List<TSubwidget> _subwidgets;

        #endregion

        public IReadOnlyList<TSubwidget> Subwidgets => _subwidgets;

        /// <summary>
        ///     Gets the parent <see cref="GameObject" /> that should be used
        ///     for any generated subwidgets.
        /// </summary>
        /// <returns>The <see cref="GameObject" /></returns>
        public abstract GameObject GetSubwidgetParent();

        /// <summary>
        ///     Implements any feature specific handling of the element when it is registered.
        /// </summary>
        /// <param name="subwidget">The subwidget to register.</param>
        protected abstract void OnRegisterSubwidget(TSubwidget subwidget);

        private void RegisterSubwidget(AppaEvent<TSubwidget>.Args args)
        {
            using (_PRF_RegisterSubwidget.Auto())
            {
                RegisterSubwidget(args.value);
            }
        }

        /// <summary>
        ///     Adds the specified subwidget to the widget's collection.
        /// </summary>
        /// <param name="subwidget">The subwidget to add.</param>
        private void RegisterSubwidget(TSubwidget subwidget)
        {
            using (_PRF_RegisterSubwidget.Auto())
            {
                _subwidgets ??= new();

                for (var i = 0; i < _subwidgets.Count; i++)
                {
                    if (_subwidgets[i] == subwidget)
                    {
                        return;
                    }
                }

                _subwidgets.Add(subwidget);

                OnRegisterSubwidget(subwidget);
            }
        }

        #region Profiling

        protected static readonly ProfilerMarker _PRF_GetSubwidgetParent =
            new ProfilerMarker(_PRF_PFX + nameof(GetSubwidgetParent));

        protected static readonly ProfilerMarker _PRF_OnRegisterSubwidget =
            new ProfilerMarker(_PRF_PFX + nameof(OnRegisterSubwidget));

        private static readonly ProfilerMarker _PRF_RegisterSubwidget =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterSubwidget));

        #endregion
    }
}
