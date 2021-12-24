using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Components.Cursors.Metadata
{
    public class ComplexCursorMetadata : CursorMetadata
    {
        #region Fields and Autoproperties

        [ReadOnly] public ComplexCursors complexCursorValue;

        [OnValueChanged(nameof(OnChanged))]
        [PreviewField(64f, ObjectFieldAlignment.Left)]
        public GameObject prefab;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField, FoldoutGroup("Movement"), PropertyRange(1f, 10000f)]
        public float maximumVelocityMagnitude;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField, FoldoutGroup("Movement"), PropertyRange(.01f, 3f)]
        public float smoothTime;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField, FoldoutGroup("Movement"), PropertyRange(1f, 10000f)]
        public float maxVelocity;

        #endregion

        public override bool isSimple => false;

        protected override void OnChanged()
        {
            if (cursorColor == default)
            {
                cursorColor = Color.white;
            }

            if ((prefab != null) && Enum.TryParse(prefab.name, out complexCursorValue))
            {
            }

            MarkAsModified();
        }
    }
}
