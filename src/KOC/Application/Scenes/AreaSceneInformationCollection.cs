using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Scriptables;
using Appalachia.Prototype.KOC.Application.Areas;
using Appalachia.Prototype.KOC.Application.Collections;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Application.Scenes
{
    [Serializable]
    [InlineEditor(Expanded = true, ObjectFieldMode = InlineEditorObjectFieldModes.Boxed)]
    [HideLabel]
    [LabelWidth(0)]
    public class AreaSceneInformationCollection : AppalachiaObjectLookupCollection<ApplicationArea,
        AreaSceneInformation, ApplicationAreaList, AreaSceneInformationList, AreaSceneInformationLookup,
        AreaSceneInformationCollection>
    {
        public override bool HasDefault => false;

        protected override ApplicationArea GetUniqueKeyFromValue(AreaSceneInformation value)
        {
            return value.Area;
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AreaSceneInformationCollection) + ".";

        

        #endregion
    }
}
