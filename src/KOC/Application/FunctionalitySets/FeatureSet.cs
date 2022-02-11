using System;
using System.Collections.Generic;
using Appalachia.Core.Objects.Dependencies;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.FunctionalitySets
{
    [InlineProperty, HideLabel]
    [FoldoutGroup("Functionality")]
    public abstract class FeatureSet<TFeature> : AppalachiaSimpleBase
        where TFeature : IApplicationFeature
    {
        public FeatureSet()
        {
            using (_PRF_FeatureSet.Auto())
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

        #endregion

        public IReadOnlyList<TFeature> Features => _features;

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

        public async AppaTask SetFeaturesToInitialState()
        {
            for (var index = 0; index < _features.Count; index++)
            {
                var feature = _features[index];

                await feature.SetToInitialState();
            }
        }

        private void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                _features ??= new List<TFeature>();
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(FeatureSet<TFeature>) + ".";

        private static readonly ProfilerMarker _PRF_RegisterFeature =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterFeature));

        private static readonly ProfilerMarker _PRF_FeatureSet = new ProfilerMarker(_PRF_PFX + "FeatureSet");

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion
    }
}
