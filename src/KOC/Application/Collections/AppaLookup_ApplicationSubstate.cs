using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Application.Areas;
using Appalachia.Prototype.KOC.Application.State;
using Appalachia.Utility.Colors;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Collections
{
    [Serializable]
    public class AppaLookup_ApplicationSubstate : AppaLookup<ApplicationArea, ApplicationSubstate,
        AppaList_ApplicationArea, AppaList_ApplicationSubstate>
    {
        protected override Color GetDisplayColor(ApplicationArea key, ApplicationSubstate value)
        {
            return Colors.WhiteSmokeGray96;
        }

        protected override string GetDisplaySubtitle(ApplicationArea key, ApplicationSubstate value)
        {
            return value.substate.ToString();
        }

        protected override string GetDisplayTitle(ApplicationArea key, ApplicationSubstate value)
        {
            return key.ToString();
        }
    }
}
