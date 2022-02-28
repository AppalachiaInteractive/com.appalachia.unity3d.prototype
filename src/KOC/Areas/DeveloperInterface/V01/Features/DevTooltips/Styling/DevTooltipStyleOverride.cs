using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Overrides.Implementations;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Styling.Overrides;
using Appalachia.UI.Core.Styling.Elements;
using Appalachia.Utility.Async;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Styling
{
    [Serializable]
    public class DevTooltipStyleOverride :
        StyleElementOverride<DevTooltipStyle, DevTooltipStyleOverride, IDevTooltipStyle>,
        IDevTooltipStyle
    {
        #region Fields and Autoproperties

        [SerializeField] private OverridableSprite _triangleSprite;
        [SerializeField] private OverridableTooltipAppearanceDirection _direction;
        [SerializeField] private OverridableFloat _distanceFromTarget;
        [SerializeField] private OverridableFloat _textPadding;
        [SerializeField] private OverridableColor _backgroundColor;
        [SerializeField] private OverridableColor _outlineColor;
        [SerializeField] private OverridableFloat _outlineThickness;
        [SerializeField] private OverridableBool _showTriangle;
        [SerializeField] private OverridableFloat _triangleSize;

        #endregion

        /// <inheritdoc />
        public override void SyncWithDefault()
        {
            using (_PRF_SyncWithDefault.Auto())
            {
                if (!_triangleSprite.Overriding)
                {
                    _triangleSprite.Value = Defaults.TriangleSprite;
                }

                if (!_direction.Overriding)
                {
                    _direction.Value = Defaults.Direction;
                }

                if (!_distanceFromTarget.Overriding)
                {
                    _distanceFromTarget.Value = Defaults.DistanceFromTarget;
                }

                if (!_textPadding.Overriding)
                {
                    _textPadding.Value = Defaults.TextPadding;
                }

                if (!_backgroundColor.Overriding)
                {
                    _backgroundColor.Value = Defaults.BackgroundColor;
                }

                if (!_outlineColor.Overriding)
                {
                    _outlineColor.Value = Defaults.OutlineColor;
                }

                if (!_outlineThickness.Overriding)
                {
                    _outlineThickness.Value = Defaults.OutlineThickness;
                }

                if (!_showTriangle.Overriding)
                {
                    _showTriangle.Value = Defaults.ShowTriangle;
                }

                if (!_triangleSize.Overriding)
                {
                    _triangleSize.Value = Defaults.TriangleSize;
                }
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(_triangleSprite), () => _triangleSprite = new(false, default));
                initializer.Do(this, nameof(_direction),      () => _direction = new(false, default));
                initializer.Do(
                    this,
                    nameof(_distanceFromTarget),
                    () => _distanceFromTarget = new(false, default)
                );
                initializer.Do(this, nameof(_textPadding),     () => _textPadding = new(false, default));
                initializer.Do(this, nameof(_backgroundColor), () => _backgroundColor = new(false, default));
                initializer.Do(this, nameof(_outlineColor),    () => _outlineColor = new(false, default));
                initializer.Do(
                    this,
                    nameof(_outlineThickness),
                    () => _outlineThickness = new(false, default)
                );
                initializer.Do(this, nameof(_showTriangle), () => _showTriangle = new(false, default));
                initializer.Do(this, nameof(_triangleSize), () => _triangleSize = new(false, default));
                
                SyncWithDefault();
            }
        }

        /// <inheritdoc />
        protected override void RegisterOverrideSubscriptions()
        {
            using (_PRF_RegisterOverrideSubscriptions.Auto())
            {
                _triangleSprite.Changed.Event += OnChanged;
                _direction.Changed.Event += OnChanged;
                _distanceFromTarget.Changed.Event += OnChanged;
                _textPadding.Changed.Event += OnChanged;
                _backgroundColor.Changed.Event += OnChanged;
                _outlineColor.Changed.Event += OnChanged;
                _outlineThickness.Changed.Event += OnChanged;
                _showTriangle.Changed.Event += OnChanged;
                _triangleSize.Changed.Event += OnChanged;
            }
        }

        #region IDevTooltipStyle Members

        public Sprite TriangleSprite
        {
            get => _triangleSprite.Get(Defaults.TriangleSprite);
            set => _triangleSprite.OverrideValue(value);
        }

        public TooltipAppearanceDirection Direction
        {
            get => _direction.Get(Defaults.Direction);
            set => _direction.OverrideValue(value);
        }

        public float DistanceFromTarget
        {
            get => _distanceFromTarget.Get(Defaults.DistanceFromTarget);
            set => _distanceFromTarget.OverrideValue(value);
        }

        public float TextPadding
        {
            get => _textPadding.Get(Defaults.TextPadding);
            set => _textPadding.OverrideValue(value);
        }

        public Color BackgroundColor
        {
            get => _backgroundColor.Get(Defaults.BackgroundColor);
            set => _backgroundColor.OverrideValue(value);
        }

        public Color OutlineColor
        {
            get => _outlineColor.Get(Defaults.OutlineColor);
            set => _outlineColor.OverrideValue(value);
        }

        public float OutlineThickness
        {
            get => _outlineThickness.Get(Defaults.OutlineThickness);
            set => _outlineThickness.OverrideValue(value);
        }

        public bool ShowTriangle
        {
            get => _showTriangle.Get(Defaults.ShowTriangle);
            set => _showTriangle.OverrideValue(value);
        }

        public float TriangleSize
        {
            get => _triangleSize.Get(Defaults.TriangleSize);
            set => _triangleSize.OverrideValue(value);
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_RegisterOverrideSubscriptions =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterOverrideSubscriptions));

        private static readonly ProfilerMarker _PRF_SyncWithDefault =
            new ProfilerMarker(_PRF_PFX + nameof(SyncWithDefault));

        #endregion
    }
}
