using System.Collections.Generic;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Areas.Common.Features;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugConditions.Model;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugConditions
{
    public sealed class DebugConditionsFeatureMetadata : AreaFeatureMetadata<DebugConditionsFeature,
        DebugConditionsFeatureMetadata, DeveloperInterfaceManager_V01, DeveloperInterfaceMetadata_V01>
    {
        #region Fields and Autoproperties

        public List<DebugConditionPacketSettings> defaultPackets;

        #endregion

#if UNITY_EDITOR
        [Button]
        public void CreateNew()
        {
            var newSettings = AppalachiaObject.CreateNew<DebugConditionPacketSettings>();
            defaultPackets.Add(newSettings);
        }
#endif
        public override void Apply(DebugConditionsFeature functionality)
        {
            using (_PRF_Apply.Auto())
            {
            }
        }
    }
}
