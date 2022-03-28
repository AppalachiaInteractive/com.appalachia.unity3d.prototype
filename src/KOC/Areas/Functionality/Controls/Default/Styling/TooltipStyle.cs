using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.UI.Core.Layout;
using Appalachia.UI.Styling.Elements;
using Appalachia.Utility.Async;
using Appalachia.Utility.Colors;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Controls.Tooltips.Styling
{
    [Serializable]
    public class TooltipStyle : StyleElementDefault<TooltipStyle, TooltipStyleOverride, ITooltipStyle>, ITooltipStyle
    {
        #region Fields and Autoproperties

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        private Sprite _triangleSprite;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private AppearanceDirection _direction;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0f, 100f)]
        private float _distanceFromTarget;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0f, 100f)]
        private float _textPadding;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private Color _backgroundColor;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private Color _outlineColor;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0f, 4f)]
        private float _outlineThickness;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private bool _showTriangle;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        [PropertyRange(2f, 36f)]
        [EnableIf(nameof(_showTriangle))]
        private float _triangleSize;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(_distanceFromTarget), () => _distanceFromTarget = 20f);
                initializer.Do(this, nameof(_textPadding), () => _textPadding = 5f);
                initializer.Do(this, nameof(_backgroundColor), () => _backgroundColor = Colors.FromHexCode("#252526"));
                initializer.Do(this, nameof(_outlineColor), () => _outlineColor = Colors.FromHexCode("#3E3E3E"));
                initializer.Do(this, nameof(_outlineThickness), () => _outlineThickness = 2f);
                initializer.Do(this, nameof(_direction), () => _direction = AppearanceDirection.Above);
                initializer.Do(this, nameof(_showTriangle), () => _showTriangle = true);
                initializer.Do(this, nameof(_triangleSize), () => _triangleSize = 18f);
            }
        }

        #region ITooltipStyle Members

        public Sprite TriangleSprite => _triangleSprite;
        public AppearanceDirection Direction => _direction;
        public float DistanceFromTarget => _distanceFromTarget;
        public float TextPadding => _textPadding;
        public Color BackgroundColor => _backgroundColor;
        public Color OutlineColor => _outlineColor;
        public float OutlineThickness => _outlineThickness;
        public bool ShowTriangle => _showTriangle;
        public float TriangleSize => _triangleSize;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_RegisterOverride =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterOverride));

        #endregion
    }
}
