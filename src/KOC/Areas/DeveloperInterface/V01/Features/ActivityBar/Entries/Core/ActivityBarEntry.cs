using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Entries.Contracts;
using Appalachia.UI.Controls.Sets.Buttons.Button;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Entries.Core
{
    public abstract class ActivityBarEntry<TEntry, TMetadata> : SingletonAppalachiaBehaviour<TEntry>,
                                                                IActivityBarEntry<TEntry, TMetadata>
        where TEntry : ActivityBarEntry<TEntry, TMetadata>
        where TMetadata : ActivityBarEntryMetadata<TEntry, TMetadata>
    {
        static ActivityBarEntry()
        {
            RegisterDependency<TMetadata>(i => _metadata = i);
        }

        #region Static Fields and Autoproperties

        private static TMetadata _metadata;

        #endregion

        #region Fields and Autoproperties

        public ButtonComponentSet button;

        #endregion

        #region IActivityBarEntry<TEntry,TMetadata> Members

        public abstract void OnClicked();

        public void UpdateActivityBarEntry()
        {
            using (_PRF_UpdateActivityBarEntry.Auto())
            {
                _metadata.UpdateActivityBarEntry(this as TEntry);
            }
        }

        public IActivityBarEntryMetadata Metadata => _metadata;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_UpdateActivityBarEntry =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateActivityBarEntry));

        #endregion
    }
}
