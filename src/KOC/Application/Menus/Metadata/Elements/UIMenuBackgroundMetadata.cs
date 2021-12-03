using System;
using Appalachia.Prototype.KOC.Application.Components.UI;
using Appalachia.Prototype.KOC.Application.Menus.Components;
using Doozy.Engine.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Menus.Metadata.Elements
{
    [Serializable]
    public class UIMenuBackgroundMetadata : UIElementMetadataBase<UIMenuBackgroundComponentSet>
    {
        #region Fields and Autoproperties

        [OnValueChanged(nameof(OnValuesChanged))]
        public Sprite sprite;

        [OnValueChanged(nameof(OnValuesChanged))]
        public Color color;

        [OnValueChanged(nameof(OnValuesChanged))]
        public Vector2 anchorMin;

        [OnValueChanged(nameof(OnValuesChanged))]
        public Vector2 anchorMax;

        #endregion

        public override void Apply(UIMenuBackgroundComponentSet component)
        {
            using (_PRF_Apply.Auto())
            {
                var rect = component.rect;
                
                rect.Reset(RectResetOptions.Transforms);

                rect.anchorMin = anchorMin;
                rect.anchorMax = anchorMax;
                component.image.sprite = sprite;
                component.image.color = color;

                _hasConfiguredAlready = true;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(UIMenuBackgroundMetadata) + ".";
        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        #endregion
    }
}
