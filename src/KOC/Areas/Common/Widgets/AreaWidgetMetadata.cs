using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Components.Styling.Fonts;
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

        [OnValueChanged(nameof(InvokeSettingsChanged))]
        public Color backgroundColor;

        [OnValueChanged(nameof(InvokeSettingsChanged))]
        public int sortOrder;

        [OnValueChanged(nameof(InvokeSettingsChanged))]
        public FontStyleOverride fontStyle;

        public bool transitionsWithFade;

        #endregion

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

                fontStyle.StyleChanged += _ => InvokeSettingsChanged();
            }
        }
    }
}
