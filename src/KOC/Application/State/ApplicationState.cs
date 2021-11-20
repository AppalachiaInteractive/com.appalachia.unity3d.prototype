using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Application.Areas;
using Appalachia.Prototype.KOC.Application.Collections;
using Appalachia.Utility.Enums;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Application.State
{
    [Serializable]
    public class ApplicationState
    {
        public ApplicationState()
        {
            Initiailze();
        }

        

        public AppaLookup_ApplicationSubstate substates;

        [HorizontalGroup("A"), SmartLabel, ReadOnly]
        public ApplicationArea currentArea;

        [HorizontalGroup("A"), SmartLabel, ReadOnly]
        public ApplicationArea nextArea;

        [NonSerialized] private bool _initialized;


        public ApplicationSubstate current
        {
            get
            {
                Initiailze();

                return substates.Get(currentArea);
            }
        }

        public ApplicationSubstate next
        {
            get
            {
                Initiailze();

                return substates.Get(nextArea);
            }
        }

        private void Initiailze()
        {
            if (_initialized)
            {
                return;
            }

            currentArea = ApplicationArea.None;
            nextArea = ApplicationArea.SplashScreen;

            substates ??= new AppaLookup_ApplicationSubstate();

            substates.PopulateEnumKeys(area => new ApplicationSubstate(area), true);

            _initialized = true;
        }
    }
}
