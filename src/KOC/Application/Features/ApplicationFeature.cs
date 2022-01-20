using Appalachia.CI.Constants;
using Appalachia.Core.Attributes;

namespace Appalachia.Prototype.KOC.Application.Features
{
    [CallStaticConstructorInEditor]
    public abstract class ApplicationFeature<TFeature, TFeatureMetadata> :
        ApplicationFunctionality<TFeature, TFeatureMetadata>,
        IApplicationFeature
        where TFeature : ApplicationFeature<TFeature, TFeatureMetadata>
        where TFeatureMetadata : ApplicationFeatureMetadata<TFeature, TFeatureMetadata>
    {
        #region Constants and Static Readonly

        protected const string GROUP_NAME = "Feature";

        #endregion

        protected override bool ParentObjectIsUI => false;
        protected override string ParentObjectName => APPASTR.ObjectNames.Features;
        protected abstract void OnApplyMetadataInternal();

        protected override void ApplyMetadataInternal()
        {
            using (_PRF_ApplyMetadataInternal.Auto())
            {
                OnApplyMetadataInternal();
            }
        }
    }
}
