using System;
using Appalachia.Core.Objects.Scriptables;
using Appalachia.Prototype.KOC.Areas;
using Appalachia.Prototype.KOC.Collections;

namespace Appalachia.Prototype.KOC.Scenes
{
    [Serializable]
    public class AreaSceneInformationCollection : AppalachiaObjectLookupCollection<ApplicationArea,
        AreaSceneInformation, ApplicationAreaList, AreaSceneInformationList, AreaSceneInformationLookup,
        AreaSceneInformationCollection>
    {
        public override bool HasDefault => false;

        protected override ApplicationArea GetUniqueKeyFromValue(AreaSceneInformation value)
        {
            return value.Area;
        }
    }
}
