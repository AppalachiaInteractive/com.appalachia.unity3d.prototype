using System.Collections.Generic;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugConditions.Model;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugConditions
{
    public sealed class DebugConditionsFeatureMetadata : DeveloperInterfaceMetadata_V01.FeatureMetadata<
        DebugConditionsFeature, DebugConditionsFeatureMetadata>
    {
        #region Fields and Autoproperties

        [OnValueChanged(nameof(OnChanged))]
        public List<DebugConditionPacketSettings> defaultPackets;

        #endregion

        protected override void UpdateFunctionality(DebugConditionsFeature functionality)
        {
            using (_PRF_Apply.Auto())
            {
            }
        }

#if UNITY_EDITOR
        [Button]
        public void CreateNew()
        {
            var newSettings = AppalachiaObject.CreateNew<DebugConditionPacketSettings>();
            defaultPackets.Add(newSettings);
        }
#endif

        protected override void SubscribeResponsiveComponents(DebugConditionsFeature target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
            }
        }
    }
}
