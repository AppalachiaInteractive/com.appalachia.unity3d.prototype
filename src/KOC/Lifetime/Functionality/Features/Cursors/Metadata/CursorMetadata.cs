using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Overrides.Implementations;
using Appalachia.Utility.Async;
using Appalachia.Utility.Colors;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Metadata
{
    [Serializable]
    public abstract class CursorMetadata<T> : AppalachiaObject<T>
        where T : CursorMetadata<T>
    {
        public abstract bool IsSimple { get; }

        #region Fields and Autoproperties

        [BoxGroup("General")]
        [SerializeField]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(8f, 256f)]
        public float size;

        [FoldoutGroup("General")]
        [SerializeField]
        [OnValueChanged(nameof(OnChanged))]
        public Vector2 hotspot;

        [FoldoutGroup("Transitions")]
        [SerializeField]
        [OnValueChanged(nameof(OnChanged))]
        [ToggleLeft]
        public bool drivenByAnimator;

        [BoxGroup("Color")]
        [SerializeField]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(.01f, 2f)]
        [HideIf(nameof(drivenByAnimator))]
        public float cursorColorChangeDuration = 0.1f;

        [BoxGroup("Color")]
        [SerializeField]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf(nameof(drivenByAnimator))]
        public OverridableColor cursorColor;

        [BoxGroup("Color")]
        [SerializeField]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf(nameof(drivenByAnimator))]
        public OverridableColor hoveringColor;

        [BoxGroup("Color")]
        [SerializeField]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf(nameof(drivenByAnimator))]
        public OverridableColor pressedColor;

        [BoxGroup("Color")]
        [SerializeField]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf(nameof(drivenByAnimator))]
        public OverridableColor disabledColor;

        [FoldoutGroup("Movement")]
        [SerializeField]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(1f, 10000f)]
        public float maximumVelocityMagnitude;

        [FoldoutGroup("Movement")]
        [SerializeField]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(.01f, 3f)]
        public float smoothTime;

        [FoldoutGroup("Movement")]
        [SerializeField]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(1f, 10000f)]
        public float maxVelocity;

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(size),    () => size = 120f);
                initializer.Do(this, nameof(hotspot), () => hotspot = Vector2.one * .5f);
                initializer.Do(
                    this,
                    nameof(cursorColorChangeDuration),
                    () => cursorColorChangeDuration = .1f
                );
                initializer.Do(
                    this,
                    nameof(maximumVelocityMagnitude),
                    () => maximumVelocityMagnitude = 5000f
                );
                initializer.Do(this, nameof(drivenByAnimator), () => drivenByAnimator = false);
                initializer.Do(this, nameof(smoothTime),       () => smoothTime = .13f);
                initializer.Do(this, nameof(maxVelocity),      () => maxVelocity = 5000f);

                initializer.Do(
                    this,
                    nameof(cursorColor),
                    cursorColor == null,
                    () => cursorColor = new(false, Color.white)
                );
                initializer.Do(
                    this,
                    nameof(hoveringColor),
                    hoveringColor == null,
                    () => hoveringColor = new(false, Color.white.ScaleV(.9f))
                );
                initializer.Do(
                    this,
                    nameof(pressedColor),
                    pressedColor == null,
                    () => pressedColor = new(false, Color.white.ScaleV(.7f))
                );
                initializer.Do(
                    this,
                    nameof(disabledColor),
                    disabledColor == null,
                    () => disabledColor = new(false, Color.white.ScaleA(.75f).ScaleV(.5f))
                );
            }
        }
    }
}
