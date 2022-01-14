using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Components.Cursors.Metadata
{
    [Serializable]
    public abstract class CursorMetadata : AppalachiaObject
    {
        #region Fields and Autoproperties

        [BoxGroup("Color")]
        [OnValueChanged(nameof(OnChanged))]
        public bool modifyColor;

        [BoxGroup("Color")] public float cursorColorChangeDuration = 0.1f;

        [BoxGroup("Color")]
        [OnValueChanged(nameof(OnChanged))]
        public float scale;

        [BoxGroup("Color")]
        [OnValueChanged(nameof(OnChanged))]
        [EnableIf(nameof(modifyColor))]
        public Color cursorColor;

        #endregion

        public abstract bool isSimple { get; }

        protected abstract void OnChanged();

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            initializer.Do(
                this,
                nameof(scale),
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        scale = 1.0f;
                    }
                }
            );

            initializer.Do(
                this,
                nameof(cursorColor),
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        cursorColor = Color.white;
                    }
                }
            );
        }

        #region Profiling

        private const string _PRF_PFX = nameof(CursorMetadata) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion
    }
}
