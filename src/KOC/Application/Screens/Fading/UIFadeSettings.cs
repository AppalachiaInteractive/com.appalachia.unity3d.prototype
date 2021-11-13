using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Scriptables;
using Appalachia.Utility.Interpolation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Screens.Fading
{
    [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
    [SmartLabelChildren]
    public class UIFadeSettings : AppalachiaObject<UIFadeSettings>
    {
        [BoxGroup("Settings")]
        [ToggleLeft]
        [SerializeField]
        private bool _startVisible = true;

        [MinMaxSlider(0.0f, 1.0f)]
        public Vector2 fadeRange = new Vector2(0.0f, 1.0f);

        [HorizontalGroup("Settings/Fades")]
        [TitleGroup("Settings/Fades/Fade In")]
        [Range(0.002f, 5f)]
        [SerializeField]
        private float _fadeInDuration = 1.0f;
        
        [TitleGroup("Settings/Fades/Fade In")]
        [SerializeField]
        private InterpolationMode _fadeIn = InterpolationMode.SmoothStep;
        
        [TitleGroup("Settings/Fades/Fade Out")]
        [Range(0.002f, 5f)]
        [SerializeField]
        private float _fadeOutDuration = 1.0f;

        [TitleGroup("Settings/Fades/Fade Out")]
        [SerializeField]
        private InterpolationMode _fadeOut = InterpolationMode.SmoothStep;
        
        public bool startVisible => _startVisible;
        public float fadeInDuration => _fadeInDuration;
        public InterpolationMode fadeIn => _fadeIn;
        public float fadeOutDuration => _fadeOutDuration;
        public InterpolationMode fadeOut => _fadeOut;

        public float minimumAlpha => fadeRange.x;
        
        public float maximumAlpha => fadeRange.y;

        private void Awake()
        {
            if (fadeRange == Vector2.zero)
            {
                fadeRange = new Vector2(0.0f, 1.0f);
            }
        }
    }
}