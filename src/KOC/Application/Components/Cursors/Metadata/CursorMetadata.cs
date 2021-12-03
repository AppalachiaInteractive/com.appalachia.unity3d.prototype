using System;
using Appalachia.Core.Scriptables;
using Appalachia.Prototype.KOC.Application.Scriptables;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Components.Cursors.Metadata
{
    public class CursorMetadata : AppalachiaApplicationObject
    {
        #region Fields and Autoproperties

        [OnValueChanged(nameof(OnChanged))]
        [PreviewField(64f, ObjectFieldAlignment.Left)] public Texture2D texture;
        
        [OnValueChanged(nameof(OnChanged))] public Vector2 hotspot;
        [OnValueChanged(nameof(OnChanged))] public bool modifyColor;

        [OnValueChanged(nameof(OnChanged))]
        public float scale;

        [OnValueChanged(nameof(OnChanged))]
        [EnableIf(nameof(modifyColor))]
        public Color cursorColor;

        [ReadOnly] public Cursors value;
        
        #endregion

        public bool HasTexture => texture != null;

        [Button]
        [EnableIf(nameof(HasTexture))]
        public void SetHotspotToCenter()
        {
            hotspot = new Vector2((int)(texture.width / 2.0f), (int)(texture.height / 2.0f));
            
            OnChanged();
        }

        private void OnChanged()
        {
            if (cursorColor == default)
            {
                cursorColor = Color.white;
            }

            if ((texture != null) && Enum.TryParse(texture.name, out value))
            {
            }
            
           this.MarkAsModified();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            initializer.Initialize(
                this,
                nameof(scale),
                () =>
                {
                    scale = 1.0f;
                }
            );

            initializer.Initialize(
                this,
                nameof(cursorColor),
                () =>
                {
                    cursorColor = Color.white;
                }
            );
        }
    }
}
