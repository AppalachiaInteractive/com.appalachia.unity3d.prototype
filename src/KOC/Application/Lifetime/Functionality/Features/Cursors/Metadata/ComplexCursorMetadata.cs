using System.Collections.Generic;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Model;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Metadata
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

        /// <inheritdoc />
        public override bool IsSimple => false;

        /// <inheritdoc />
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
