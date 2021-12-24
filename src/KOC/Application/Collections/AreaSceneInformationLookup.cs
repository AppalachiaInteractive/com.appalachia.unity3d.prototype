using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Application.Areas;
using Appalachia.Prototype.KOC.Application.Scenes;
using Appalachia.Utility.Colors;
using Appalachia.Utility.Strings;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Collections
{
    [Serializable]
    public class AreaSceneInformationLookup : AppaLookup<ApplicationArea, AreaSceneInformation,
        ApplicationAreaList, AreaSceneInformationList>
    {
        protected override Color GetDisplayColor(ApplicationArea key, AreaSceneInformation value)
        {
            return Colors.WhiteSmokeGray96;
        }

        protected override string GetDisplaySubtitle(ApplicationArea key, AreaSceneInformation value)
        {
            return ZString.Format(
                "Initial: {0}",
                value.entrySceneReference == null ? "Not Set" : value.entrySceneReference.name
            );
        }

        protected override string GetDisplayTitle(ApplicationArea key, AreaSceneInformation value)
        {
            return key.ToString();
        }
    }
}
