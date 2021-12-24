using System;
using Appalachia.Prototype.KOC.Application.Components.UI;
using Appalachia.Prototype.KOC.Application.Menus.Components;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Menus.Metadata.Elements
{
    [Serializable]
    public class UIMenuBackgroundMetadata : UIElementMetadataBase<UIMenuBackgroundComponentSet>
    {
        #region Fields and Autoproperties

        public Sprite sprite;

        public Color color;

        public Vector2 anchorMin;

        public Vector2 anchorMax;

        #endregion

        public override void Apply(
            GameObject parent,
            string baseName,
            ref UIMenuBackgroundComponentSet component)
        {
            using (_PRF_Apply.Auto())
            {
                using (var scope = initializer.Scope(
                           this,
                           nameof(component),
                           (component.image == null) ||
                           (component.rect == null) ||
                           (component.gameObject == null)
                       ))
                {
                    if (scope.ShouldInitialize)
                    {
                        component.Configure(parent, baseName);
                        scope.MarkInitialized();
                    }
                }

                using (var scope = initializer.Scope(this, nameof(RectResetOptions)))
                {
                    if (scope.ShouldInitialize)
                    {
                        component.rect.Reset(RectResetOptions.Transforms);
                        scope.MarkInitialized();
                    }
                }

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

                await initializer.Do(this, nameof(component.image), () => { });
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(UIMenuBackgroundMetadata) + ".";
        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        #endregion
    }
}
