using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Styling.Fonts;
using Appalachia.Prototype.KOC.Application.Styling.OnScreenButtons;
using Appalachia.Utility.Async;
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

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

#if UNITY_EDITOR
            await initializer.Do(
                this,
                nameof(defaultFont),
                defaultFont == null,
                () => { defaultFont = FontStyle.LoadOrCreateNew("Default Font"); }
            );

            await initializer.Do(
                this,
                nameof(OnScreenButtonStyle),
                onScreenButtonStyle == null,
                () =>
                {
                    onScreenButtonStyle = OnScreenButtonStyle.LoadOrCreateNew(nameof(OnScreenButtonStyle));
                }
            );
#endif
        }
    }
}
