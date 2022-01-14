using System;
using Appalachia.Core.Objects.Scriptables;
using Appalachia.Prototype.KOC.Areas;
using Appalachia.Prototype.KOC.Collections;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Scenes
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

        #region Profiling

        #endregion
    }
}
