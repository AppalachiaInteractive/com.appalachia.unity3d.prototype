using System;
using Appalachia.Prototype.KOC.Application.Collections;
using Appalachia.Utility.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Application.State
{
    [Serializable]
    public class ApplicationAreaStateCollection
    {
        private ApplicationAreaStateCollection()
        {
        }

        #region Fields and Autoproperties

        [FormerlySerializedAs("_substates")]
        [FormerlySerializedAs("substates")]
        [SerializeField]
        private AppaLookup_ApplicationAreaState _areas;

        [NonSerialized] private bool _initialized;

        #endregion

        public int ActivateCompleteCount => _areas.CountValues_NoAlloc(v =>   (v.State == ApplicationAreaStates.Activate) && (v.Substate == ApplicationAreaSubstates.Complete));
        public int ActivateFailedCount => _areas.CountValues_NoAlloc(v =>     (v.State == ApplicationAreaStates.Activate) && (v.Substate == ApplicationAreaSubstates.Failed));
        public int ActivateInProgressCount => _areas.CountValues_NoAlloc(v => (v.State == ApplicationAreaStates.Activate) && (v.Substate == ApplicationAreaSubstates.InProgress));
        public int LoadCompleteCount => _areas.CountValues_NoAlloc(v =>   (v.State == ApplicationAreaStates.Load) && (v.Substate == ApplicationAreaSubstates.Complete));
        public int LoadFailedCount => _areas.CountValues_NoAlloc(v =>     (v.State == ApplicationAreaStates.Load) && (v.Substate == ApplicationAreaSubstates.Failed));
        public int LoadInProgressCount => _areas.CountValues_NoAlloc(v => (v.State == ApplicationAreaStates.Load) && (v.Substate == ApplicationAreaSubstates.InProgress));
        public int UnloadCompleteCount => _areas.CountValues_NoAlloc(v =>   (v.State == ApplicationAreaStates.Unload) && (v.Substate == ApplicationAreaSubstates.Complete));
        public int UnloadFailedCount => _areas.CountValues_NoAlloc(v =>     (v.State == ApplicationAreaStates.Unload) && (v.Substate == ApplicationAreaSubstates.Failed));
        public int UnloadInProgressCount => _areas.CountValues_NoAlloc(v => (v.State == ApplicationAreaStates.Unload) && (v.Substate == ApplicationAreaSubstates.InProgress));
        public int CompleteCount => _areas.CountValues_NoAlloc(v =>   v.Substate == ApplicationAreaSubstates.Complete);
        public int FailedCount => _areas.CountValues_NoAlloc(v =>     v.Substate == ApplicationAreaSubstates.Failed);
        public int InProgressCount => _areas.CountValues_NoAlloc(v => v.Substate == ApplicationAreaSubstates.InProgress);
        public int ActivateCount => _areas.CountValues_NoAlloc(v => v.State == ApplicationAreaStates.Activate);
        public int LoadCount => _areas.CountValues_NoAlloc(v =>     v.State == ApplicationAreaStates.Load);
        public int UnloadCount => _areas.CountValues_NoAlloc(v => v.State == ApplicationAreaStates.Unload);

        public AppaLookup_ApplicationAreaState Areas
        {
            get => _areas;
            set => _areas = value;
        }

        public static ApplicationAreaStateCollection CreateNew()
        {
            var instance = new ApplicationAreaStateCollection();
            instance.Initialize();

            return instance;
        }

        internal void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _areas ??= new AppaLookup_ApplicationAreaState();

            _areas.PopulateEnumKeys(area => new ApplicationAreaState(area), true);

            _initialized = true;
        }
    }
}
