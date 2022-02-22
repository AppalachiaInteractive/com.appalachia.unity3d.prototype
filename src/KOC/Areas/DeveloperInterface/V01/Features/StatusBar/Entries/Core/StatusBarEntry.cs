using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Entries.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Widgets;
using Appalachia.UI.Controls.Extensions;
using Appalachia.UI.Controls.Sets.Buttons.Button;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Entries.Core
{
    [CallStaticConstructorInEditor]
    [RequireComponent(typeof(RectTransform))]
    public abstract class
        StatusBarEntry<TEntry, TMetadata, TButtonSet, TButtonSetData> : SingletonAppalachiaBehaviour<TEntry>,
                                                                        IStatusBarEntry<TEntry, TMetadata>
        where TEntry : StatusBarEntry<TEntry, TMetadata, TButtonSet, TButtonSetData>
        where TMetadata : StatusBarEntryMetadata<TEntry, TMetadata, TButtonSet, TButtonSetData>
        where TButtonSet : BaseButtonComponentSet<TButtonSet, TButtonSetData>, IButtonComponentSet, new()
        where TButtonSetData : BaseButtonComponentSetData<TButtonSet, TButtonSetData>, IButtonComponentSetData
    {
        static StatusBarEntry()
        {
            RegisterDependency<StatusBarWidget>(i => { _widget = i; });

            RegisterDependency<TMetadata>(i => { _metadata = i; });
        }

        #region Static Fields and Autoproperties

        private static StatusBarWidget _widget;

        [PropertyOrder(-10)]
        [ShowInInspector, InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        private static TMetadata _metadata;

        #endregion

        #region Fields and Autoproperties

        public TButtonSet button;

        #endregion

        protected StatusBarWidget Widget => _widget;

        /// <inheritdoc />
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                RectTransform.FullScreen(true);
            }
        }

        #region IStatusBarEntry<TEntry,TMetadata> Members

        public abstract void OnClicked();

        [Button]
        public virtual void UpdateStatusBarEntry()
        {
            using (_PRF_UpdateStatusBarEntry.Auto())
            {
                _metadata.UpdateStatusBarEntry(this as TEntry);
            }
        }

        public IStatusBarEntryMetadata Metadata => _metadata;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_UpdateStatusBarEntry =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateStatusBarEntry));

        #endregion
    }
}
