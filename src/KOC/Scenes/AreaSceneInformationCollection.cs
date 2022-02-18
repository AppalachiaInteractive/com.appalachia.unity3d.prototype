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
        /// <inheritdoc />
        public override bool HasDefault => false;

        /// <inheritdoc />
        protected override ApplicationArea GetUniqueKeyFromValue(AreaSceneInformation value)
        {
            return value.Area;
        }
    }
}
