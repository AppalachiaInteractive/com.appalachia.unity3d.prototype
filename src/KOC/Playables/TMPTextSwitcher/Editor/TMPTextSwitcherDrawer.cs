#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Playables.TMPTextSwitcher.Editor
{
    [CustomPropertyDrawer(typeof(TMPTextSwitcherBehaviour))]
    public class TMPTextSwitcherDrawer : PropertyDrawer
    {
        /// <inheritdoc />
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var fieldCount = 3;
            return fieldCount * EditorGUIUtility.singleLineHeight;
        }

        /// <inheritdoc />
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var colorProp = property.FindPropertyRelative("color");
            var fontSizeProp = property.FindPropertyRelative("fontSize");
            var textProp = property.FindPropertyRelative("text");

            var singleFieldRect = new Rect(
                position.x,
                position.y,
                position.width,
                EditorGUIUtility.singleLineHeight
            );
            EditorGUI.PropertyField(singleFieldRect, textProp);

            singleFieldRect.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(singleFieldRect, fontSizeProp);

            singleFieldRect.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(singleFieldRect, colorProp);
        }
    }
}

#endif
