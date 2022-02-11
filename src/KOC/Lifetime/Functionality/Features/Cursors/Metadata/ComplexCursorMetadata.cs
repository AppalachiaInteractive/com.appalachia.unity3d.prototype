using System.Collections.Generic;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Model;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Metadata
{
    public partial class ComplexCursorMetadata : CursorMetadata<ComplexCursorMetadata>
    {
        #region Fields and Autoproperties

        [FormerlySerializedAs("complexCursorValue")]
        [ReadOnly]
        public ComplexCursors value;

        [OnValueChanged(nameof(OnChanged))]
        public RuntimeAnimatorController animatorController;

        [OnValueChanged(nameof(OnChanged))]
        public List<Sprite> sprites;

        #endregion

        public override bool IsSimple => false;

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(sprites), sprites == null, () => sprites = new List<Sprite>());
            }
        }
    }
}
