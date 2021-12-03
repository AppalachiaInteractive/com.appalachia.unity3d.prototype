using System;
using Appalachia.Prototype.KOC.Application.Menus.Components;
using Doozy.Engine.UI;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;
using ButtonStyle = Appalachia.Prototype.KOC.Application.Styling.Buttons.ButtonStyle;

namespace Appalachia.Prototype.KOC.Application.Menus.Metadata.Elements
{
    [Serializable]
    public class UIMenuButtonMetadata : UIElementMetadataBase<UIMenuButtonComponentSet>
    {
        #region Fields and Autoproperties

        [OnValueChanged(nameof(OnValuesChanged))]
        public Sprite icon;

        [OnValueChanged(nameof(OnValuesChanged))]
        public string buttonCategory;

        [OnValueChanged(nameof(OnValuesChanged))]
        public string buttonName;

        [OnValueChanged(nameof(OnValuesChanged))]
        public string text;

        [OnValueChanged(nameof(OnValuesChanged))]
        public ButtonStyle style;

        #endregion

        public override void Apply(UIMenuButtonComponentSet components)
        {
            using (_PRF_Apply.Auto())
            {
                if (hasConfiguredAlready && !haveValuesChanged)
                {
                    return;
                }

                components.buttonWrapper.button.targetGraphic = components.buttonWrapper.composite;

                components.buttonWrapper.composite.graphics = new Graphic[]
                {
                    components.buttonImage.image, components.buttonText.textMesh
                };

                /*var colorBlock = components.buttonWrapper.button.colors;

                colorBlock.normalColor = style.normalColor;
                colorBlock.highlightedColor = style.highlightedColor;
                colorBlock.pressedColor = style.pressedColor;
                colorBlock.selectedColor = style.selectedColor;
                colorBlock.disabledColor = style.disabledColor;
                colorBlock.colorMultiplier = style.colorMultiplier;
                colorBlock.fadeDuration = style.fadeDuration;

                components.buttonText.textMesh.text = text;
                components.buttonText.textMesh.color = Color.white;

                components.buttonImage.image.sprite = icon;
                components.buttonImage.image.color = Color.white;

                components.buttonWrapper.doozyButton.TargetLabel = TargetLabel.TextMeshPro;
                components.buttonWrapper.doozyButton.TextMeshProLabel = components.buttonText.textMesh;

                components.buttonWrapper.doozyButton.ButtonCategory = buttonCategory;
                components.buttonWrapper.doozyButton.ButtonName = buttonName;

                components.buttonText.textMesh.fontSize = style.fontSize;
                components.buttonText.textMesh.alignment = style.alignment;*/

                _hasConfiguredAlready = true;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(UIMenuButtonMetadata) + ".";
        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        #endregion
    }
}
