using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Application.Areas;
using Appalachia.Prototype.KOC.Application.State;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Collections
{
    [Serializable]
    public class AppaLookup_ApplicationAreaState : AppaLookup<ApplicationArea, ApplicationAreaState,
        AppaList_ApplicationArea, AppaList_ApplicationAreaState>
    {
        protected override Color GetDisplayColor(ApplicationArea key, ApplicationAreaState value)
        {
            return value.GetStateColorInternal(value.State, value.Substate);
        }

        protected override string GetDisplaySubtitle(ApplicationArea key, ApplicationAreaState value)
        {
            return $"{value.State}:{value.Substate}";
        }

        protected override string GetDisplayTitle(ApplicationArea key, ApplicationAreaState value)
        {
            return key.ToString();
        }
    }
}
