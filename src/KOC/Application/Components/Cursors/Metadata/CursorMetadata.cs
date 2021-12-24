using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Components.Cursors.Metadata
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

            await initializer.Do(this, nameof(scale), () => { scale = 1.0f; });

            await initializer.Do(this, nameof(cursorColor), () => { cursorColor = Color.white; });
        }
    }
}
