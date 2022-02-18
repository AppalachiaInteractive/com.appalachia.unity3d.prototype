using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Application.State;
using Appalachia.Prototype.KOC.Areas;
using Appalachia.Utility.Strings;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Collections
{
    [Serializable]
    public class ApplicationAreaStateLookup : AppaLookup<ApplicationArea, ApplicationAreaState,
        ApplicationAreaList, ApplicationAreaStateList>
    {
        /// <inheritdoc />
        protected override Color GetDisplayColor(ApplicationArea key, ApplicationAreaState value)
        {
            return value.GetStateColorInternal(value.State, value.Substate);
        }

        /// <inheritdoc />
        protected override string GetDisplaySubtitle(ApplicationArea key, ApplicationAreaState value)
        {
            return ZString.Format("{0}:{1}", value.State, value.Substate);
        }

        /// <inheritdoc />
        protected override string GetDisplayTitle(ApplicationArea key, ApplicationAreaState value)
        {
            return key.ToString();
        }
    }
}
