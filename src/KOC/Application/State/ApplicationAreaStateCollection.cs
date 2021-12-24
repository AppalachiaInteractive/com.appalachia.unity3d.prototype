using System;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Application.Collections;
using Appalachia.Utility.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.State
{
    [Serializable]
    public class ApplicationAreaStateCollection : AppalachiaSimpleBase
    {
        private ApplicationAreaStateCollection()
        {
        }

        #region Fields and Autoproperties

        [SerializeField, HideLabel]
        [ListDrawerSettings(
            Expanded = true,
            DraggableItems = false,
            HideAddButton = true,
            HideRemoveButton = true,
            NumberOfItemsPerPage = 5
        )]
        private ApplicationAreaStateLookup _areas;

        [NonSerialized] private bool _initialized;

        #endregion

        public int ActivateCompleteCount =>
            _areas.CountValues_NoAlloc(
                v => (v.State == ApplicationAreaStates.Activate) &&
                     (v.Substate == ApplicationAreaSubstates.Complete)
            );

        public int ActivateCount =>
            _areas.CountValues_NoAlloc(v => v.State == ApplicationAreaStates.Activate);

        public int ActivateFailedCount =>
            _areas.CountValues_NoAlloc(
                v => (v.State == ApplicationAreaStates.Activate) &&
                     (v.Substate == ApplicationAreaSubstates.Failed)
            );

        public int ActivateInProgressCount =>
            _areas.CountValues_NoAlloc(
                v => (v.State == ApplicationAreaStates.Activate) &&
                     (v.Substate == ApplicationAreaSubstates.InProgress)
            );

        public int CompleteCount =>
            _areas.CountValues_NoAlloc(v => v.Substate == ApplicationAreaSubstates.Complete);

        public int FailedCount =>
            _areas.CountValues_NoAlloc(v => v.Substate == ApplicationAreaSubstates.Failed);

        public int InProgressCount =>
            _areas.CountValues_NoAlloc(v => v.Substate == ApplicationAreaSubstates.InProgress);

        public int LoadCompleteCount =>
            _areas.CountValues_NoAlloc(
                v => (v.State == ApplicationAreaStates.Load) &&
                     (v.Substate == ApplicationAreaSubstates.Complete)
            );

        public int LoadCount => _areas.CountValues_NoAlloc(v => v.State == ApplicationAreaStates.Load);

        public int LoadFailedCount =>
            _areas.CountValues_NoAlloc(
                v => (v.State == ApplicationAreaStates.Load) &&
                     (v.Substate == ApplicationAreaSubstates.Failed)
            );

        public int LoadInProgressCount =>
            _areas.CountValues_NoAlloc(
                v => (v.State == ApplicationAreaStates.Load) &&
                     (v.Substate == ApplicationAreaSubstates.InProgress)
            );

        public int UnloadCompleteCount =>
            _areas.CountValues_NoAlloc(
                v => (v.State == ApplicationAreaStates.Unload) &&
                     (v.Substate == ApplicationAreaSubstates.Complete)
            );

        public int UnloadCount => _areas.CountValues_NoAlloc(v => v.State == ApplicationAreaStates.Unload);

        public int UnloadFailedCount =>
            _areas.CountValues_NoAlloc(
                v => (v.State == ApplicationAreaStates.Unload) &&
                     (v.Substate == ApplicationAreaSubstates.Failed)
            );

        public int UnloadInProgressCount =>
            _areas.CountValues_NoAlloc(
                v => (v.State == ApplicationAreaStates.Unload) &&
                     (v.Substate == ApplicationAreaSubstates.InProgress)
            );

        public ApplicationAreaStateLookup Areas
        {
            get => _areas;
            set => _areas = value;
        }

        public static ApplicationAreaStateCollection CreateNew(ApplicationManager manager)
        {
            var instance = new ApplicationAreaStateCollection();
            instance.Initialize(manager);

            return instance;
        }

        internal void Initialize(ApplicationManager manager)
        {
            if (_initialized)
            {
                return;
            }

            _areas ??= new ApplicationAreaStateLookup();

            _areas.SetObjectOwnership(manager);

            _areas.PopulateEnumKeys(area => new ApplicationAreaState(area), clear: true);

            _initialized = true;
        }
    }
}
