using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Application.Features;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.FunctionalitySets
{
    public abstract partial class FeatureManager<T, TFeatureSet, TIFeature> : AppalachiaBehaviour<T>
        where T : FeatureManager<T, TFeatureSet, TIFeature>
        where TFeatureSet : FeatureSet<TIFeature>
        where TIFeature : IApplicationFeature
    {
        #region Fields and Autoproperties

        [ShowInInspector, HideReferenceObjectPicker]
        private TFeatureSet _functionality;

        #endregion

        public TFeatureSet Functionality
        {
            get => _functionality;
            set => _functionality = value;
        }

        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            await AppaTask.WaitUntil(() => _functionality != null);

            for (var featureIndex = 0; featureIndex < Functionality.Features.Count; featureIndex++)
            {
                var feature = Functionality.Features[featureIndex];

                await AppaTask.WaitUntil(() => feature.FullyInitialized && feature.HasBeenEnabled);

                feature.SortWidgets();
                feature.SortServices();
            }
        }

        protected void ApplyAllMetadata()
        {
            using (_PRF_ApplyAllMetadata.Auto())
            {
                for (var featureIndex = 0; featureIndex < Functionality.Features.Count; featureIndex++)
                {
                    var feature = Functionality.Features[featureIndex];

                    feature.ApplyMetadata();
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_ApplyAllMetadata =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyAllMetadata));

        #endregion
    }
}
