using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.UI.Core.Components.Subsets;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Widgets
{
    public sealed class ActivityBarWidgetMetadata : DeveloperInterfaceMetadata_V01.WidgetMetadata<
        ActivityBarWidget, ActivityBarWidgetMetadata, ActivityBarFeature, ActivityBarFeatureMetadata>
    {
        #region Constants and Static Readonly

        private const string BOTTOM_LAYOUT_GROUP_ENTRIES_OBJ_NAME = "Bottom Activity Bar Entries";

        private const string TOP_LAYOUT_GROUP_ENTRIES_OBJ_NAME = "Top Activity Bar Entries";

        #endregion

        #region Fields and Autoproperties

        [BoxGroup(APPASTR.GroupNames.Dimensions)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0.03f, 0.07f)]
        public float width;

        [OnValueChanged(nameof(OnChanged))]
        public VerticalLayoutGroupSubsetData topLayoutGroup;

        [OnValueChanged(nameof(OnChanged))]
        public VerticalLayoutGroupSubsetData bottomLayoutGroup;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(width), () => width = 0.03f);

                initializer.Do(
                    this,
                    nameof(topLayoutGroup),
                    topLayoutGroup == null,
                    () => topLayoutGroup = new VerticalLayoutGroupSubsetData(this)
                );

                initializer.Do(
                    this,
                    nameof(bottomLayoutGroup),
                    bottomLayoutGroup == null,
                    () => bottomLayoutGroup = new VerticalLayoutGroupSubsetData(this)
                );
            }
        }

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(ActivityBarWidget target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(target);

                topLayoutGroup.Changed.Event += OnChanged;
                bottomLayoutGroup.Changed.Event += OnChanged;
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(ActivityBarWidget widget)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
                base.UpdateFunctionalityInternal(widget);

                for (var entryIndex = 0; entryIndex < widget.TopActivityBarEntries.Count; entryIndex++)
                {
                    var activityBarEntry = widget.TopActivityBarEntries[entryIndex];
                    activityBarEntry.UpdateActivityBarEntry();
                }

                for (var entryIndex = 0; entryIndex < widget.BottomActivityBarEntries.Count; entryIndex++)
                {
                    var activityBarEntry = widget.BottomActivityBarEntries[entryIndex];
                    activityBarEntry.UpdateActivityBarEntry();
                }

                VerticalLayoutGroupSubsetData.RefreshAndUpdateComponentSubset(
                    ref bottomLayoutGroup,
                    this,
                    ref widget.bottomActivityBarLayoutGroup,
                    widget.ActivityBarEntryParent,
                    BOTTOM_LAYOUT_GROUP_ENTRIES_OBJ_NAME
                );

                VerticalLayoutGroupSubsetData.RefreshAndUpdateComponentSubset(
                    ref topLayoutGroup,
                    this,
                    ref widget.topActivityBarLayoutGroup,
                    widget.ActivityBarEntryParent,
                    TOP_LAYOUT_GROUP_ENTRIES_OBJ_NAME
                );

                widget.topActivityBarLayoutGroup.RectTransform.SetSiblingIndex(0);
                widget.bottomActivityBarLayoutGroup.RectTransform.SetSiblingIndex(1);
            }
        }
    }
}
