using System;
using System.Collections.Generic;
using Appalachia.CI.Constants;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Dependencies;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features;
using Appalachia.Prototype.KOC.Application.Features.Services;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.FunctionalitySets
{
    [InlineProperty, HideLabel]
    [NonSerializable]
    public abstract class FeatureFunctionalitySet<TIService, TIWidget> : AppalachiaSimpleBase
        where TIService : IApplicationService
        where TIWidget : IApplicationWidget
    {
        protected FeatureFunctionalitySet()
        {
            using (_PRF_FeatureFunctionalitySet.Auto())
            {
                Initialize();
            }
        }

        #region Fields and Autoproperties

        [InlineProperty]
        [HideLabel]
        [ShowInInspector]
        [Title(APPASTR.ObjectNames.Services)]
        private List<TIService> _services;

        [InlineProperty]
        [HideLabel]
        [ShowInInspector]
        [Title(APPASTR.ObjectNames.Widgets)]
        private List<TIWidget> _widgets;

        [InlineProperty]
        [HideLabel]
        [ShowInInspector]
        [Title(APPASTR.ObjectNames.Features)]
        private List<IApplicationFeature> _features;

        #endregion

        public IReadOnlyList<IApplicationFeature> Features
        {
            get
            {
                _features ??= new();
                return _features;
            }
        }

        public IReadOnlyList<TIService> Services
        {
            get
            {
                _services ??= new();
                return _services;
            }
        }

        public IReadOnlyList<TIWidget> Widgets
        {
            get
            {
                _widgets ??= new();
                return _widgets;
            }
        }

        public void AddService<TService>(TService service)
            where TService : MonoBehaviour, TIService
        {
            using (_PRF_AddService.Auto())
            {
                var found = false;
                for (var i = 0; i < _services.Count; i++)
                {
                    var current = _services[i];

                    var currentMonoBehaviour = current as MonoBehaviour;
                    if (currentMonoBehaviour == service)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    _services.Add(service);
                }
            }
        }

        public void AddWidget<TWidget>(TWidget widget)
            where TWidget : MonoBehaviour, TIWidget
        {
            using (_PRF_AddWidget.Auto())
            {
                var found = false;
                for (var i = 0; i < _widgets.Count; i++)
                {
                    var current = _widgets[i];

                    var currentMonoBehaviour = current as MonoBehaviour;
                    if (currentMonoBehaviour == widget)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    _widgets.Add(widget);
                }
            }
        }

        public void RegisterFeature<TDependency>(
            AppalachiaRepositoryDependencyTracker dependencyTracker,
            SingletonAppalachiaBehaviour<TDependency>.InstanceAvailableHandler handler)
            where TDependency : SingletonAppalachiaBehaviour<TDependency>,
            IRepositoryDependencyTracker<TDependency>, IApplicationFeature
        {
            using (_PRF_RegisterFeature.Auto())
            {
                Initialize();

                var wrapper = new SingletonAppalachiaBehaviour<TDependency>.InstanceAvailableHandler(
                    i =>
                    {
                        try
                        {
                            _features.Add(i);
                            handler?.Invoke(i);
                        }
                        catch (Exception ex)
                        {
                            Context.Log.Error(
                                ZString.Format(
                                    "Error executing the {0} registration action for {1}.",
                                    typeof(TDependency).FormatForLogging(),
                                    dependencyTracker.Owner.FormatForLogging()
                                ),
                                null,
                                ex
                            );
                        }
                    }
                );

                dependencyTracker.RegisterDependency(wrapper);
            }
        }

        public void RegisterService<TDependency>(
            AppalachiaRepositoryDependencyTracker dependencyTracker,
            SingletonAppalachiaBehaviour<TDependency>.InstanceAvailableHandler handler = null)
            where TDependency : SingletonAppalachiaBehaviour<TDependency>,
            IRepositoryDependencyTracker<TDependency>, TIService
        {
            using (_PRF_RegisterService.Auto())
            {
                Initialize();

                var wrapper = new SingletonAppalachiaBehaviour<TDependency>.InstanceAvailableHandler(
                    i =>
                    {
                        try
                        {
                            _services.Add(i);
                            handler?.Invoke(i);
                        }
                        catch (Exception ex)
                        {
                            Context.Log.Error(
                                ZString.Format(
                                    "Error executing the {0} registration action for {1}.",
                                    typeof(TDependency).FormatForLogging(),
                                    dependencyTracker.Owner.FormatForLogging()
                                ),
                                null,
                                ex
                            );
                        }
                    }
                );

                dependencyTracker.RegisterDependency(wrapper);
            }
        }

        public void RegisterWidget<TDependency>(
            AppalachiaRepositoryDependencyTracker dependencyTracker,
            SingletonAppalachiaBehaviour<TDependency>.InstanceAvailableHandler handler)
            where TDependency : SingletonAppalachiaBehaviour<TDependency>,
            IRepositoryDependencyTracker<TDependency>, TIWidget
        {
            using (_PRF_RegisterWidget.Auto())
            {
                Initialize();

                var wrapper = new SingletonAppalachiaBehaviour<TDependency>.InstanceAvailableHandler(
                    i =>
                    {
                        try
                        {
                            _widgets.Add(i);
                            handler?.Invoke(i);
                        }
                        catch (Exception ex)
                        {
                            Context.Log.Error(
                                ZString.Format(
                                    "Error executing the {0} registration action for {1}.",
                                    typeof(TDependency).FormatForLogging(),
                                    dependencyTracker.Owner.FormatForLogging()
                                ),
                                null,
                                ex
                            );
                        }
                    }
                );

                dependencyTracker.RegisterDependency(wrapper);
            }
        }

        private void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                _features ??= new();
                _widgets ??= new();
                _services ??= new();
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(FeatureFunctionalitySet<TIService, TIWidget>) + ".";

        private static readonly ProfilerMarker _PRF_AddService =
            new ProfilerMarker(_PRF_PFX + nameof(AddService));

        private static readonly ProfilerMarker _PRF_AddWidget =
            new ProfilerMarker(_PRF_PFX + nameof(AddWidget));

        private static readonly ProfilerMarker _PRF_RegisterFeature =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterFeature));

        private static readonly ProfilerMarker _PRF_RegisterWidget =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterWidget));

        private static readonly ProfilerMarker _PRF_RegisterService =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterService));

        private static readonly ProfilerMarker _PRF_FeatureFunctionalitySet =
            new ProfilerMarker(_PRF_PFX + "FeatureFunctionalitySet");

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion
    }
}
