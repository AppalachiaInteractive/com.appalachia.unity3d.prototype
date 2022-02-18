using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Model;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Metadata
{
    public class SimpleCursorMetadata : CursorMetadata<SimpleCursorMetadata>
    {
        #region Fields and Autoproperties

        [FormerlySerializedAs("simpleCursorValue")]
        [ReadOnly]
        public SimpleCursors value;

        [OnValueChanged(nameof(OnChanged))]
        [PreviewField(64f, ObjectFieldAlignment.Left)]
        public Sprite texture;

        #endregion

        /// <inheritdoc />
        public override bool IsSimple => true;

        public bool HasTexture => texture != null;
    }
}
