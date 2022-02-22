using System;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Entries.Contracts;
using Appalachia.UI.Controls.Sets.Buttons.Button;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Entries.Core
{
    [Serializable]
    public abstract class ActivityBarEntryMetadata<TEntry, TMetadata> : SingletonAppalachiaObject<TMetadata>,
                                                                        IActivityBarEntryMetadata<TEntry,
                                                                            TMetadata>
        where TEntry : ActivityBarEntry<TEntry, TMetadata>
        where TMetadata : ActivityBarEntryMetadata<TEntry, TMetadata>
    {
        #region Fields and Autoproperties

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public ButtonComponentSetData _button;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private bool _enabled;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private ActivityBarSection _section;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private int _priority;

        #endregion

        /// <summary>
        ///     Given an activity bar entry, applies this metadata to it.
        /// </summary>
        public void UpdateActivityBarEntry(TEntry entry)
        {
            using (_PRF_UpdateActivityBarEntry.Auto())
            {
                // TODO 1. check if _enabled and do whatever we should if not. (for _enabled)
                // TODO 2. Call the ButtonComponentSetData.RefreshAndUpdateComponentSet method (for _button)
                // TODO 3. use _section to determine if we are in top or bottom layout group.
                // TODO    add us to the right layout group.
                // TODO 4. figure out how to sort layout group by priority. 
                // TODO   ** sorting might be better to happen in the widget itself **
            }
        }

        #region IActivityBarEntryMetadata<TEntry,TMetadata> Members

        public ButtonComponentSetData Button => _button;

        public bool Enabled => _enabled;

        public ActivityBarSection Section => _section;

        public int Priority => _priority;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_UpdateActivityBarEntry =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateActivityBarEntry));

        #endregion
    }
}
