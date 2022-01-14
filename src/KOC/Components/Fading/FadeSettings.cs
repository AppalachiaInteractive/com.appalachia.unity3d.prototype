using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using Appalachia.Utility.Interpolation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Components.Fading
{
    [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
    [SmartLabelChildren]
    [Serializable]
    public class FadeSettings : AppalachiaObject<FadeSettings>
    {
        #region Fields and Autoproperties

        [BoxGroup("Settings")]
        [ToggleLeft]
        [SerializeField]
        private bool _passiveMode;

        [BoxGroup("Settings")]
        [SerializeField]
        [EnableIf(nameof(_passiveMode))]
        private InterpolationMode _passiveFadeMode = InterpolationMode.SmoothStep;

        [HorizontalGroup("Settings/Start")]
        [ToggleLeft]
        [SerializeField]
        [DisableIf(nameof(_passiveMode))]
        private bool _updateAtStart;

        [HorizontalGroup("Settings/Start")]
        [ToggleLeft]
        [SerializeField]
        [DisableIf(nameof(_passiveMode))]
        [EnableIf(nameof(_updateAtStart))]
        private bool _startVisible = true;

        [MinMaxSlider(0.0f, 1.0f)]
        public Vector2 fadeRange = new Vector2(0.0f, 1.0f);

        [HorizontalGroup("Settings/Fades")]
        [TitleGroup("Settings/Fades/Fade In")]
        [Range(0.002f, 5f)]
        [SerializeField]
        [DisableIf(nameof(_passiveMode))]
        private float _fadeInDuration = 1.0f;

        [TitleGroup("Settings/Fades/Fade In")]
        [SerializeField]
        [DisableIf(nameof(_passiveMode))]
        private InterpolationMode _fadeIn = InterpolationMode.SmoothStep;

        [TitleGroup("Settings/Fades/Fade Out")]
        [Range(0.002f, 5f)]
        [SerializeField]
        [DisableIf(nameof(_passiveMode))]
        private float _fadeOutDuration = 1.0f;

        [TitleGroup("Settings/Fades/Fade Out")]
        [SerializeField]
        [DisableIf(nameof(_passiveMode))]
        private InterpolationMode _fadeOut = InterpolationMode.SmoothStep;

        #endregion

        public bool passiveMode => _passiveMode;
        public bool startVisible => _startVisible;
        public bool updateAtStart => _updateAtStart;
        public float fadeInDuration => _fadeInDuration;
        public float fadeOutDuration => _fadeOutDuration;

        public float maximumAlpha => fadeRange.y;

        public float minimumAlpha => fadeRange.x;
        public InterpolationMode fadeIn => _fadeIn;
        public InterpolationMode fadeOut => _fadeOut;
        public InterpolationMode passiveFadeMode => _passiveFadeMode;

        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            if (fadeRange == Vector2.zero)
            {
                fadeRange = new Vector2(0.0f, 1.0f);
            }
        }
    }
}
