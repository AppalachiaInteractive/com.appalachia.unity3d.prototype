using Appalachia.Prototype.KOC.Application.Styling.Fonts;
using Appalachia.Prototype.KOC.Application.Styling.OnScreenButtons;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Application.Styling
{
    public class ApplicationUIStyle : ApplicationStyle
    {
        #region Fields and Autoproperties

        [InlineEditor(InlineEditorObjectFieldModes.Boxed), FoldoutGroup("General")]
        public FontStyle defaultFont;

        [InlineEditor(InlineEditorObjectFieldModes.Foldout), FoldoutGroup(nameof(onScreenButtonStyle))]
        public OnScreenButtonStyle onScreenButtonStyle;

        #endregion

        #region Event Functions

        protected override void Awake()
        {
            base.Awake();

            Initialize();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            Initialize();
        }

        #endregion

        protected override void Initialize()
        {
            base.Initialize();

            initializer.Initialize(
                this,
                nameof(defaultFont),
                defaultFont == null,
                () => { defaultFont = LoadOrCreateNew<FontStyle>("Default Font"); }
            );

            initializer.Initialize(
                this,
                nameof(OnScreenButtonStyle),
                onScreenButtonStyle == null,
                () =>
                {
                    onScreenButtonStyle = LoadOrCreateNew<OnScreenButtonStyle>(nameof(OnScreenButtonStyle));
                }
            );
        }
    }
}
