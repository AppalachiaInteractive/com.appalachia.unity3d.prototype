using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Sets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Entries.Contracts;
using Appalachia.UI.Controls.Sets.Buttons.Button;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Entries.Core
{
    [Serializable]
    [CallStaticConstructorInEditor]
    public abstract class
        StatusBarEntryMetadata<TEntry, TMetadata, TButtonSet, TButtonSetData> :
            SingletonAppalachiaObject<TMetadata>,
            IStatusBarEntryMetadata<TEntry, TMetadata>
        where TEntry : StatusBarEntry<TEntry, TMetadata, TButtonSet, TButtonSetData>
        where TMetadata : StatusBarEntryMetadata<TEntry, TMetadata, TButtonSet, TButtonSetData>
        where TButtonSet : BaseButtonComponentSet<TButtonSet, TButtonSetData>, IButtonComponentSet, new()
        where TButtonSetData : BaseButtonComponentSetData<TButtonSet, TButtonSetData>, IButtonComponentSetData
    {
        static StatusBarEntryMetadata()
        {
        }

        #region Fields and Autoproperties

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public TButtonSetData _button;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private bool _enabled;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private StatusBarSection _section;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private int _priority;

        #endregion

        /// <summary>
        ///     Given an activity bar entry, applies this metadata to it.
        /// </summary>
        public void UpdateStatusBarEntry(TEntry entry)
        {
            using (_PRF_UpdateStatusBarEntry.Auto())
            {
                if (!entry.enabled)
                {
                    if (entry.button != null)
                    {
                        entry.button?.DisableSet();
                    }

                    return;
                }

                ComponentSetData<TButtonSet, TButtonSetData>.RefreshAndUpdateComponentSet(
                    ref _button,
                    ref entry.button,
                    entry.gameObject,
                    entry.name
                );

                var textSubset = entry.button.buttonText;
                var text = textSubset.TextMeshProUGUI;

                text.autoSizeTextContainer = true;
                
                var sizeDelta = text.rectTransform.sizeDelta;
                sizeDelta.x = text.preferredWidth;
                
                text.rectTransform.sizeDelta = sizeDelta;
                
                LayoutRebuilder.MarkLayoutForRebuild(text.rectTransform);
            }
        }


        /// <inheritdoc />
        protected override async AppaTask Initialize(
            Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(_enabled),  () => _enabled= true);
                initializer.Do(this, nameof(_section),  () => _section = StatusBarSection.Left);
                initializer.Do(this, nameof(_priority), () => _priority = 100);
            }
        }

        #region IStatusBarEntryMetadata<TEntry,TMetadata> Members

        public IButtonComponentSetData Button => _button;

        public bool Enabled => _enabled;

        public StatusBarSection Section => _section;

        public int Priority => _priority;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_UpdateStatusBarEntry =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateStatusBarEntry));

        #endregion
    }
}
