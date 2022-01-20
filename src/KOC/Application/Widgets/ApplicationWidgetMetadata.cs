using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.UI.Controls.Sets.Background;
using Appalachia.UI.Controls.Sets.Canvas;
using Appalachia.UI.Core.Components.Sets.Data;
using Appalachia.UI.Core.Styling.Fonts;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Application.Widgets
{
    public abstract class
        ApplicationWidgetMetadata<TWidget, TWidgetMetadata> : ApplicationFunctionalityMetadata<TWidget,
            TWidgetMetadata>
        where TWidget : ApplicationWidget<TWidget, TWidgetMetadata>
        where TWidgetMetadata : ApplicationWidgetMetadata<TWidget, TWidgetMetadata>

    {
        #region Fields and Autoproperties

        [FoldoutGroup(APPASTR.GroupNames.Common)]
        [FoldoutGroup(APPASTR.GroupNames.Common + "/" + APPASTR.GroupNames.General)]
        [OnValueChanged(nameof(InvokeSettingsChanged))]
        public RectTransformData rectTransformData;

        [FoldoutGroup(APPASTR.GroupNames.Common + "/" + APPASTR.GroupNames.Canvas), HideLabel]
        [OnValueChanged(nameof(InvokeSettingsChanged))]
        public CanvasComponentSetStyle canvasStyle;

        [FoldoutGroup(APPASTR.GroupNames.Common + "/" + APPASTR.GroupNames.Background), HideLabel]
        [OnValueChanged(nameof(InvokeSettingsChanged))]
        public BackgroundComponentSetStyle backgroundStyle;

        [FoldoutGroup(APPASTR.GroupNames.Common + "/" + APPASTR.GroupNames.Style), HideLabel]
        [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        [OnValueChanged(nameof(InvokeSettingsChanged))]
        public FontStyleOverride fontStyle;

        [FoldoutGroup(APPASTR.GroupNames.Common + "/" + APPASTR.GroupNames.Transitions)]
        [OnValueChanged(nameof(InvokeSettingsChanged))]
        public bool transitionsWithFade;

        [FoldoutGroup(APPASTR.GroupNames.Common + "/" + APPASTR.GroupNames.Transitions)]
        [OnValueChanged(nameof(InvokeSettingsChanged))]
        [PropertyRange(0f, 1f)]
        public float animationDuration;

        #endregion

        public override void Apply(TWidget functionality)
        {
            using (_PRF_Apply.Auto())
            {
                rectTransformData.ConfigureComponent(functionality.rectTransform);
                canvasStyle.ConfigureComponents(functionality.canvas);
                backgroundStyle.ConfigureComponents(functionality.background);
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(
                    this,
                    nameof(RectTransformData),
                    rectTransformData == null,
                    () => { rectTransformData = new RectTransformData(this); }
                );

                initializer.Do(
                    this,
                    nameof(CanvasComponentSetStyle),
                    canvasStyle == null,
                    () =>
                    {
                        canvasStyle = LoadOrCreateNew<CanvasComponentSetStyle>(
                            typeof(TWidget).Name + nameof(CanvasComponentSetStyle),
                            ownerType: typeof(ApplicationManager)
                        );
                    }
                );

                initializer.Do(
                    this,
                    nameof(BackgroundComponentSetStyle),
                    backgroundStyle == null,
                    () =>
                    {
                        backgroundStyle = LoadOrCreateNew<BackgroundComponentSetStyle>(
                            typeof(TWidget).Name + nameof(BackgroundComponentSetStyle),
                            ownerType: typeof(ApplicationManager)
                        );
                    }
                );

                initializer.Do(
                    this,
                    nameof(FontStyleOverride),
                    fontStyle == null,
                    () =>
                    {
                        fontStyle = LoadOrCreateNew<FontStyleOverride>(
                            typeof(TWidget).Name + nameof(FontStyleOverride),
                            ownerType: typeof(ApplicationManager)
                        );
                    }
                );

                initializer.Do(this, nameof(animationDuration), () => { animationDuration = .2f; });

                rectTransformData.SettingsChanged += _ => InvokeSettingsChanged();
                canvasStyle.SettingsChanged += _ => InvokeSettingsChanged();
                backgroundStyle.SettingsChanged += _ => InvokeSettingsChanged();
                fontStyle.SettingsChanged += _ => InvokeSettingsChanged();
            }
        }
    }
}
