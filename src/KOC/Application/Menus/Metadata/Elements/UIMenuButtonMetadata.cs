using System;
using Appalachia.Prototype.KOC.Application.Menus.Components;
using Appalachia.Prototype.KOC.Application.Styling.Buttons;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Menus.Metadata.Elements
{
    [Serializable]
    public class UIMenuButtonMetadata : UIElementMetadataBase<UIMenuButtonComponentSet>
    {
        #region Fields and Autoproperties

        public Sprite icon;

        public string buttonCategory;

        public string buttonName;

        public string text;

        public ButtonStyle style;

        #endregion

        public override void Apply(GameObject parent, string baseName, ref UIMenuButtonComponentSet component)
        {
            using (_PRF_Apply.Auto())
            {
                if ((component.buttonImage.image == null) ||
                    (component.buttonImage.rect == null) ||
                    (component.buttonText.textMesh == null) ||
                    (component.rect == null) ||
                    (component.gameObject == null))
                {
                    component.Configure(parent, baseName);
                }

                if (component.buttonWrapper.composite.graphics == null)
                {
                    component.buttonWrapper.button.targetGraphic = component.buttonWrapper.composite;

                    component.buttonWrapper.composite.graphics = new Graphic[]
                    {
                        component.buttonImage.image, component.buttonText.textMesh
                    };
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(UIMenuButtonMetadata) + ".";
        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        #endregion
    }
}
