using Appalachia.Prototype.KOC.Components.OnScreenButtons;
using Appalachia.Prototype.KOC.Components.OnScreenButtons.Controls;
using Appalachia.UI.Styling.Elements;
using Appalachia.UI.Styling.Fonts;
using TMPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


namespace Appalachia.Prototype.KOC.Components.Styling.OnScreenButtons
{
    public interface IOnScreenButtonStyle : IStyleElement
    {
        public Color SpriteColor { get; }
        public Color TextColor { get; }
        public FontStyleOverride Font { get; }
        public OnScreenButtonSpriteStyle SpriteStyle { get; }
        public OnScreenButtonTextStyle TextStyle { get; }

        public void Apply(ControlButtonMetadata metadata, Image component)
        {
            using (_PRF_Apply.Auto())
            {
                component.sprite = metadata.GetSprite(SpriteStyle);
                component.color = SpriteColor;
                component.preserveAspect = true;
            }
        }

        public void Apply(InputAction action, ControlButtonMetadata metadata, TextMeshProUGUI component)
        {
            using (_PRF_Apply.Auto())
            {
                Font.ToApplicable.Apply(component);

                component.text = metadata.GetDisplayText(action, TextStyle);

                if (TextColor != Color.clear)
                {
                    component.color = TextColor;
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(IOnScreenButtonStyle) + ".";

        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        #endregion
    }
}
