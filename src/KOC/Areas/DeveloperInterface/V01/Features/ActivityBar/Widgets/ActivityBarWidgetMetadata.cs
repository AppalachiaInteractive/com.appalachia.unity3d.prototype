using System.Collections.Generic;
using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Model;
using Appalachia.UI.Controls.Sets.Button;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Widgets
{
    public sealed class ActivityBarWidgetMetadata : DeveloperInterfaceMetadata_V01.WidgetMetadata<
        ActivityBarWidget, ActivityBarWidgetMetadata, ActivityBarFeature, ActivityBarFeatureMetadata>
    {
        #region Fields and Autoproperties

        [BoxGroup(APPASTR.GroupNames.Dimensions)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0.015f, 0.045f)]
        public float width;

        [FormerlySerializedAs("buttonStyle")]
        [BoxGroup(APPASTR.GroupNames.Style)]
        [OnValueChanged(nameof(OnChanged))]
        public ButtonComponentSetData _buttonData;

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
                    nameof(_buttonData),
                    _buttonData == null,
                    () =>
                    {
                        _buttonData = AppalachiaObject.LoadOrCreateNew<ButtonComponentSetData>(
                            nameof(ActivityBarWidget) + nameof(ButtonComponentSetData),
                            ownerType: typeof(ApplicationManager)
                        );
                    }
                );
            }
        }

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(ActivityBarWidget target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(target);

                _buttonData.Changed.Event += OnChanged;
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(ActivityBarWidget widget)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
                base.UpdateFunctionalityInternal(widget);

                ApplyToButtons(widget.TopEntries,    widget.TopButtons,    widget.TopButtonParent);
                ApplyToButtons(widget.BottomEntries, widget.BottomButtons, widget.BottomButtonParent);
            }
        }

        private void ApplyToButtons(
            IReadOnlyList<ActivityBarEntry> entries,
            IReadOnlyList<ButtonComponentSet> buttons,
            GameObject parent)
        {
            using (_PRF_ApplyToButtons.Auto())
            {
                if ((entries == null) || (buttons == null))
                {
                    return;
                }

                for (var index = 0; index < entries.Count; index++)
                {
                    var button = buttons[index];
                    var entry = entries[index];

                    ButtonComponentSetData.RefreshAndUpdateComponentSet(
                        ref _buttonData,
                        ref button,
                        parent,
                        nameof(ActivityBarWidget)
                    );

                    button.ButtonIcon.sprite = entry.sprite;
                    button.TooltipData.Text = entry.tooltip;
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_ApplyToButtons =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyToButtons));

        #endregion
    }
}
