using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.UI.Core.Styling.Fonts;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.Common.Widgets
{
    public abstract class
        AreaWidgetMetadata<TWidget, TWidgetMetadata, TAreaManager, TAreaMetadata> : AreaFunctionalityMetadata<
            TWidget, TWidgetMetadata, TAreaManager, TAreaMetadata>
        where TWidget : AreaWidget<TWidget, TWidgetMetadata, TAreaManager, TAreaMetadata>
        where TWidgetMetadata : AreaWidgetMetadata<TWidget, TWidgetMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
        #region Fields and Autoproperties

        [BoxGroup(APPASTR.GroupNames.Color)]
        [OnValueChanged(nameof(InvokeSettingsChanged))]
        public Color backgroundColor;

        [BoxGroup(APPASTR.GroupNames.Rendering)]
        [OnValueChanged(nameof(InvokeSettingsChanged))]
        public int sortOrder;

        [BoxGroup(APPASTR.GroupNames.Style)]
        [OnValueChanged(nameof(InvokeSettingsChanged))]
        public FontStyleOverride fontStyle;

        [BoxGroup(APPASTR.GroupNames.Transitions)]
        public bool transitionsWithFade;

        #endregion

        public override void Apply(TWidget functionality)
        {
            using (_PRF_Apply.Auto())
            {
                functionality.components.background.color = backgroundColor;
                functionality.components.background.rectTransform.FullScreen(true);

                var overrideSortOrder = sortOrder != 0;
                functionality.components.canvas.overrideSorting = overrideSortOrder;
                functionality.components.canvas.sortingOrder = sortOrder;
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(
                    this,
                    nameof(FontStyleOverride),
                    fontStyle == null,
                    () => { fontStyle = LoadOrCreateNew<FontStyleOverride>(typeof(TWidget).Name); }
                );

                initializer.Do(this, nameof(backgroundColor), () => { backgroundColor = Color.white; });

                fontStyle.SettingsChanged += _ => InvokeSettingsChanged();
            }
        }
    }
}
