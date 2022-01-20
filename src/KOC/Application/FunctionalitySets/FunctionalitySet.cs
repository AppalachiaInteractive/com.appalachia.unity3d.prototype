using System;
using System.Collections.Generic;
using Appalachia.Core.Objects.Dependencies;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.FunctionalitySets
{
    [InlineProperty, HideLabel]
    [FoldoutGroup("Functionality")]
    public abstract class FunctionalitySet<TFeature, TWidget, TService> : AppalachiaSimpleBase
    {
        public FunctionalitySet()
        {
            using (_PRF_FunctionalitySet.Auto())
            {
                Initialize();
            }
        }

        #region Fields and Autoproperties

        [InlineProperty]
        [HideLabel]
        [ShowInInspector]
        [Title("Features")]
        private List<TFeature> _features;

        [InlineProperty]
        [HideLabel]
        [ShowInInspector]
        [Title("Services")]
        private List<TService> _services;

        [InlineProperty]
        [HideLabel]
        [ShowInInspector]
        [Title("Widgets")]
        private List<TWidget> _widgets;

        #endregion

        public IReadOnlyList<TFeature> Features => _features;
        public IReadOnlyList<TService> Services => _services;
        public IReadOnlyList<TWidget> Widgets => _widgets;

        public void RegisterFeature<TDependency>(
            AppalachiaRepositoryDependencyTracker dependencyTracker,
            SingletonAppalachiaBehaviour<TDependency>.InstanceAvailableHandler handler)
            where TDependency : SingletonAppalachiaBehaviour<TDependency>,
            IRepositoryDependencyTracker<TDependency>, TFeature
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
            IRepositoryDependencyTracker<TDependency>, TService
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
            IRepositoryDependencyTracker<TDependency>, TWidget
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
                _widgets ??= new List<TWidget>();
                _features ??= new List<TFeature>();
                _services ??= new List<TService>();
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(FunctionalitySet<TFeature, TWidget, TService>) + ".";

        private static readonly ProfilerMarker _PRF_RegisterFeature =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterFeature));

        private static readonly ProfilerMarker _PRF_RegisterWidget =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterWidget));

        private static readonly ProfilerMarker _PRF_RegisterService =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterService));

        private static readonly ProfilerMarker _PRF_FunctionalitySet =
            new ProfilerMarker(_PRF_PFX + "FunctionalitySet");

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion
    }
}
