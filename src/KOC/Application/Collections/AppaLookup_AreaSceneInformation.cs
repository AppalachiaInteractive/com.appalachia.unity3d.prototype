using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Application.Areas;
using Appalachia.Prototype.KOC.Application.Scenes;
using Appalachia.Utility.Colors;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Collections
{
    [Serializable]
    public class AppaLookup_AreaSceneInformation : AppaLookup<ApplicationArea, AreaSceneInformation,
        AppaList_ApplicationArea, AppaList_AreaSceneInformation>
    {
        protected override Color GetDisplayColor(ApplicationArea key, AreaSceneInformation value)
        {
            return Colors.WhiteSmokeGray96;
        }

        protected override string GetDisplaySubtitle(ApplicationArea key, AreaSceneInformation value)
        {
            return $"Initial: {(value.entrySceneReference == null ? "Not Set" : value.entrySceneReference.name)}";
        }

        protected override string GetDisplayTitle(ApplicationArea key, AreaSceneInformation value)
        {
            return key.ToString();
        }
    }
}
