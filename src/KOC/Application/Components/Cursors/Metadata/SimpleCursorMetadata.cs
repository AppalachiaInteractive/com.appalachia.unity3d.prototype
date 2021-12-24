using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Application.Components.Cursors.Metadata
{
    public class SimpleCursorMetadata : CursorMetadata
    {
        #region Fields and Autoproperties

        [FormerlySerializedAs("value")]
        [ReadOnly]
        public SimpleCursors simpleCursorValue;

        [OnValueChanged(nameof(OnChanged))]
        [PreviewField(64f, ObjectFieldAlignment.Left)]
        public Texture2D texture;

        [OnValueChanged(nameof(OnChanged))]
        public Vector2 hotspot;

        #endregion

        public override bool isSimple => true;

        public bool HasTexture => texture != null;

        [ButtonGroup]
        [EnableIf(nameof(HasTexture))]
        public void SetHotspotToCenter()
        {
            if (texture != null)
            {
                hotspot = new Vector2((int)(texture.width / 2.0f), (int)(texture.height / 2.0f));
            }

            OnChanged();
        }

        [ButtonGroup]
        protected override void OnChanged()
        {
            if (cursorColor == default)
            {
                cursorColor = Color.white;
            }

            if ((texture != null) && Enum.TryParse(texture.name, out simpleCursorValue))
            {
            }

            MarkAsModified();
        }
    }
}
