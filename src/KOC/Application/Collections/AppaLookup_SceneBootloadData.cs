using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Application.Areas;
using Appalachia.Prototype.KOC.Application.Scenes;
using Appalachia.Utility.Colors;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Collections
{
    [Serializable]
    public class AppaLookup_SceneBootloadData : AppaLookup<ApplicationArea, SceneBootloadData,
        AppaList_ApplicationArea, AppaList_SceneBootloadData>
    {
        protected override Color GetDisplayColor(ApplicationArea key, SceneBootloadData value)
        {
            return Colors.WhiteSmokeGray96;
        }

        protected override string GetDisplaySubtitle(ApplicationArea key, SceneBootloadData value)
        {
            return $"Initial: {(value.entryScene == null ? "Not Set" : value.entryScene.name)}";
        }

        protected override string GetDisplayTitle(ApplicationArea key, SceneBootloadData value)
        {
            return key.ToString();
        }
    }
}
