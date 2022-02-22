using System;
using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Utility.Events;
using Appalachia.Core.Objects.Dependencies;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Events.Extensions;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.FunctionalitySets
{
    [InlineProperty, HideLabel]
    [NonSerializable]
    public abstract class FeatureSet<TFeature> : AppalachiaSimpleBase
        where TFeature : IApplicationFeature
    {
        protected FeatureSet()
        {
            using (_PRF_FeatureSet.Auto())
            {
                Initialize();
            }
        }

        #region Fields and Autoproperties

        public AppaEvent.Data AllFeaturesAvailable;

        [InlineProperty]
        [HideLabel]
        [ShowInInspector]
        [ListDrawerSettings(
            HideAddButton = true,
            HideRemoveButton = true,
            IsReadOnly = true,
            DraggableItems = false,
            ShowPaging = false,
            NumberOfItemsPerPage = 100,
            Expanded = true,
            ShowItemCount = true
        )]
        private List<TFeature> _features;

        private List<Type> _registeredFeatures;

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

                _registeredFeatures.Add(typeof(TDependency));

                var wrapper = new SingletonAppalachiaBehaviour<TDependency>.InstanceAvailableHandler(
                    i =>
                    {
                        try
                        {
                            _features.Add(i);
                            handler?.Invoke(i);

                            ValidateFeatureAvailability();
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
                _registeredFeatures ??= new();
                _features ??= new List<TFeature>();
            }
        }

        private void ValidateFeatureAvailability()
        {
            using (_PRF_ValidateFeatureAvailability.Auto())
            {
                if (_features.Count == _registeredFeatures.Count)
                {
                    AllFeaturesAvailable.RaiseEvent();
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(FeatureSet<TFeature>) + ".";

        private static readonly ProfilerMarker _PRF_ValidateFeatureAvailability =
            new ProfilerMarker(_PRF_PFX + nameof(ValidateFeatureAvailability));

        private static readonly ProfilerMarker _PRF_RegisterFeature =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterFeature));

        private static readonly ProfilerMarker _PRF_FeatureSet = new ProfilerMarker(_PRF_PFX + "FeatureSet");

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion
    }
}
