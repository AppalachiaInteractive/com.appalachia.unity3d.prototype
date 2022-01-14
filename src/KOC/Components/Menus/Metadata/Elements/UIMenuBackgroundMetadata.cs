using System;
using Appalachia.Prototype.KOC.Components.Menus.Components;
using Appalachia.Prototype.KOC.Extensions;
using Unity.Profiling;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Prototype.KOC.Components.Menus.Metadata.Elements
{
    [Serializable]
    public class
        UIMenuBackgroundMetadata : UIElementMetadataBase<UIMenuBackgroundMetadata,
            UIMenuBackgroundComponentSet>
    {
        public UIMenuBackgroundMetadata(Object owner) : base(owner)
        {
        }

        #region Fields and Autoproperties

        public Sprite sprite;

        public Color color;

        public Vector2 anchorMin;

        public Vector2 anchorMax;

        #endregion

        public override void Apply(GameObject parent, string baseName, UIMenuBackgroundComponentSet component)
        {
            using (_PRF_Apply.Auto())
            {
                initializer.Do(
                    this,
                    nameof(component),
                    (component.image == null) || (component.rect == null) || (component.gameObject == null),
                    () => { component.Configure(parent, baseName); }
                );

                initializer.Do(
                    this,
                    nameof(RectResetOptions),
                    () => { component.rect.Reset(RectResetOptions.Transforms); }
                );
                if (component.rect.anchorMin != anchorMin)
                {
                    component.rect.anchorMin = anchorMin;
                }

                if (component.rect.anchorMax != anchorMax)
                {
                    component.rect.anchorMax = anchorMax;
                }

                if (component.image.sprite != sprite)
                {
                    component.image.sprite = sprite;
                }

                if (component.image.color != color)
                {
                    component.image.color = color;
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        #endregion
    }
}
