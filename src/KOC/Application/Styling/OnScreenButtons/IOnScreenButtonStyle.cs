using Appalachia.Prototype.KOC.Application.Input.OnScreenButtons;
using Appalachia.Prototype.KOC.Application.Input.OnScreenButtons.Controls;
using Appalachia.Prototype.KOC.Application.Styling.Base;
using Appalachia.Prototype.KOC.Application.Styling.Fonts;
using TMPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Styling.OnScreenButtons
{
    public interface IOnScreenButtonStyle : IApplicationStyle
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
