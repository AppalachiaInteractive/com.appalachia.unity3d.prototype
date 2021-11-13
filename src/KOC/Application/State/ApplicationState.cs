using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Application.Areas;
using Appalachia.Prototype.KOC.Application.Collections;
using Appalachia.Utility.Enums;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Application.State
{
    [Serializable]
    public class ApplicationState
    {
        public ApplicationState()
        {
            currentArea = ApplicationArea.None;
            nextArea = ApplicationArea.SplashScreen;
            
            substates.PopulateEnumKeys(area => new ApplicationSubstate(area), true);
        }

        [HorizontalGroup("A"), SmartLabel, ReadOnly]
        public ApplicationArea currentArea;

        [HorizontalGroup("A"), SmartLabel, ReadOnly]
        public ApplicationArea nextArea;

        public AppaLookup_ApplicationSubstate substates;

        public ApplicationSubstate current => substates.Get(currentArea);
        public ApplicationSubstate next => substates.Get(nextArea);
    }
}
