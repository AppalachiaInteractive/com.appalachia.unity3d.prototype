using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Components.Styling.OnScreenButtons;
using Appalachia.UI.Styling.Fonts;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Components.Styling
{
    public class ApplicationUIStyle : AppalachiaObject<ApplicationUIStyle>
    {
        #region Fields and Autoproperties

        [FoldoutGroup("General")]
        public FontStyle defaultFont;

        [FoldoutGroup(nameof(onScreenButtonStyle))]
        public OnScreenButtonStyle onScreenButtonStyle;

        #endregion

        /// <inheritdoc />
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
