using System;
using Appalachia.Prototype.KOC.Components.Menus.Components;
using Appalachia.Prototype.KOC.Components.Styling.Buttons;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Appalachia.Prototype.KOC.Components.Menus.Metadata.Elements
{
    [Serializable]
    public class UIMenuButtonMetadata : UIElementMetadataBase<UIMenuButtonMetadata, UIMenuButtonComponentSet>
    {
        public UIMenuButtonMetadata(Object owner) : base(owner)
        {
        }

        #region Fields and Autoproperties

        public Sprite icon;

        public string buttonCategory;

        public string buttonName;

        public string text;

        public ButtonStyle style;

        #endregion

        public override void Apply(GameObject parent, string baseName, UIMenuButtonComponentSet component)
        {
            using (_PRF_Apply.Auto())
            {
                initializer.Do(
                    this,
                    nameof(component),
                    (component.buttonImage.image == null) ||
                    (component.buttonImage.rect == null) ||
                    (component.buttonText.textMesh == null) ||
                    (component.rect == null) ||
                    (component.gameObject == null),
                    () => { component.Configure(parent, baseName); }
                );

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

        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        #endregion
    }
}
