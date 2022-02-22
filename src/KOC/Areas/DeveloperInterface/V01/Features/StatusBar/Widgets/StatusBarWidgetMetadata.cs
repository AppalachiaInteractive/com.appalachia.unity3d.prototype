using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.UI.Core.Components.Subsets;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Widgets
{
    public sealed class StatusBarWidgetMetadata : DeveloperInterfaceMetadata_V01.WidgetMetadata<
        StatusBarWidget, StatusBarWidgetMetadata, StatusBarFeature, StatusBarFeatureMetadata>
    {
        #region Constants and Static Readonly

        private const string LEFT_LAYOUT_GROUP_ENTRIES_OBJ_NAME = "Left Status Bar Entries";

        private const string RIGHT_LAYOUT_GROUP_ENTRIES_OBJ_NAME = "Right Status Bar Entries";

        #endregion

        #region Fields and Autoproperties

        [BoxGroup(APPASTR.GroupNames.Dimensions)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0.015f, 0.045f)]
        public float height;

        [OnValueChanged(nameof(OnChanged))]
        public HorizontalLayoutGroupSubsetData leftLayoutGroup;

        [OnValueChanged(nameof(OnChanged))]
        public HorizontalLayoutGroupSubsetData rightLayoutGroup;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            initializer.Do(this, nameof(height), () => height = 0.03f);

            initializer.Do(
                this,
                nameof(leftLayoutGroup),
                leftLayoutGroup == null,
                () => leftLayoutGroup = new HorizontalLayoutGroupSubsetData(this)
            );

            initializer.Do(
                this,
                nameof(rightLayoutGroup),
                rightLayoutGroup == null,
                () => rightLayoutGroup = new HorizontalLayoutGroupSubsetData(this)
            );
        }

        protected override void SubscribeResponsiveComponents(StatusBarWidget target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(target);

                leftLayoutGroup.Changed.Event += OnChanged;
                rightLayoutGroup.Changed.Event += OnChanged;
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(StatusBarWidget widget)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
                base.UpdateFunctionalityInternal(widget);

                for (var entryIndex = 0; entryIndex < widget.LeftStatusBarEntries.Count; entryIndex++)
                {
                    var entry = widget.LeftStatusBarEntries[entryIndex];
                    entry.UpdateStatusBarEntry();
                }

                for (var entryIndex = 0; entryIndex < widget.RightStatusBarEntries.Count; entryIndex++)
                {
                    var entry = widget.RightStatusBarEntries[entryIndex];
                    entry.UpdateStatusBarEntry();
                }

                HorizontalLayoutGroupSubsetData.RefreshAndUpdateComponentSubset(
                    ref rightLayoutGroup,
                    this,
                    ref widget.rightStatusBarLayoutGroup,
                    widget.StatusBarEntryParent,
                    RIGHT_LAYOUT_GROUP_ENTRIES_OBJ_NAME
                );

                HorizontalLayoutGroupSubsetData.RefreshAndUpdateComponentSubset(
                    ref leftLayoutGroup,
                    this,
                    ref widget.leftStatusBarLayoutGroup,
                    widget.StatusBarEntryParent,
                    LEFT_LAYOUT_GROUP_ENTRIES_OBJ_NAME
                );

                // TODO 3. use _section to determine if we are in top or bottom layout group.
                // TODO    add us to the right layout group.
                widget.ValidateEntries();

                // TODO 4. figure out how to sort layout group by priority. 
                // TODO   ** sorting might be better to happen in the widget itself **
                widget.SortEntriesByPriority();

                widget.leftStatusBarLayoutGroup.RectTransform.SetSiblingIndex(0);
                widget.rightStatusBarLayoutGroup.RectTransform.SetSiblingIndex(1);
            }
        }
    }
}
