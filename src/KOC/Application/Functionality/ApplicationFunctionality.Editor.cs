#if UNITY_EDITOR
using Appalachia.CI.Constants;
using Appalachia.Core.Attributes;
using Appalachia.Core.Attributes.Editing;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Functionality
{
    [CallStaticConstructorInEditor]
    [SmartLabelChildren]
    [ExecuteAlways]
    public abstract partial class ApplicationFunctionality<TFunctionality, TFunctionalityMetadata, TManager>
    {
        #region Constants and Static Readonly

        protected const string FEATURE_BUTTON_GROUP = nameof(ApplyMetadataButton);

        #endregion

        [ButtonGroup(FEATURE_BUTTON_GROUP)]
        [LabelText(APPASTR.Apply_Metadata)]
        [GUIColor(nameof(MetadataColor))]
        private void ApplyMetadataButton()
        {
            using (_PRF_ApplyMetadataButton.Auto())
            {
                ApplyMetadata();
            }
        }

        #region Profiling

        protected static readonly ProfilerMarker _PRF_ApplyMetadataButton =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyMetadataButton));

        #endregion
    }
}

#endif
