using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Components.Styling.Fonts;
using Appalachia.Prototype.KOC.Components.Styling.OnScreenButtons;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Components.Styling
{
    public class ApplicationUIStyle : ApplicationStyle
    {
        #region Fields and Autoproperties

        [InlineEditor(InlineEditorObjectFieldModes.Boxed), FoldoutGroup("General")]
        public FontStyle defaultFont;

        [InlineEditor(InlineEditorObjectFieldModes.Foldout), FoldoutGroup(nameof(onScreenButtonStyle))]
        public OnScreenButtonStyle onScreenButtonStyle;

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

#if UNITY_EDITOR
            initializer.Do(
                this,
                nameof(defaultFont),
                defaultFont == null,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        defaultFont = FontStyle.LoadOrCreateNew("Default Font");
                    }
                }
            );

            initializer.Do(
                this,
                nameof(OnScreenButtonStyle),
                onScreenButtonStyle == null,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        onScreenButtonStyle =
                            OnScreenButtonStyle.LoadOrCreateNew(nameof(OnScreenButtonStyle));
                    }
                }
            );
#endif
        }
    }
}
