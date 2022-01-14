/*
#if UNITY_EDITOR
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Attributes;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Strings;
using UnityEditor;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Editor
{
    [CallStaticConstructorInEditor]
    internal static class RuntimeGraphEditorStyle
    {
        #region Static Fields and Autoproperties

        private static GUISkin m_skin;
        private static GUIStyle m_foldoutStyle;
        private static GUIStyle m_headerStyle1;
        private static GUIStyle m_headerStyle2;

        private static string path;
        private static Texture2D _debuggerLogoTexture;

        private static Texture2D _managerLogoTexture;

        #endregion

        public static GUISkin Skin => m_skin;
        public static GUIStyle FoldoutStyle => m_foldoutStyle;
        public static GUIStyle HeaderStyle1 => m_headerStyle1;
        public static GUIStyle HeaderStyle2 => m_headerStyle2;
        public static Texture2D DebuggerLogoTexture => _debuggerLogoTexture;

        public static Texture2D ManagerLogoTexture => _managerLogoTexture;

        private static void SetGuiStyleFontColor(GUIStyle guiStyle, Color color)
        {
            guiStyle.normal.textColor = color;
            guiStyle.hover.textColor = color;
            guiStyle.active.textColor = color;
            guiStyle.focused.textColor = color;
            guiStyle.onNormal.textColor = color;
            guiStyle.onHover.textColor = color;
            guiStyle.onActive.textColor = color;
            guiStyle.onFocused.textColor = color;
        }

        private static void InitializeRuntimeGraphEditorStyle()
        {
            GUIStyle foldoutStyle;
            try
            {
                foldoutStyle = EditorStyles.foldout;
                EditorApplication.update -= InitializeRuntimeGraphEditorStyle;
            }
            catch
            {
                return;
            }

            var boldLabelStyle = EditorStyles.boldLabel;

            var managerLogoGuid = AssetDatabaseManager.FindAssets(
                ZString.Format("Manager_Logo_{0}", EditorGUIUtility.isProSkin ? "White" : "Dark")
            )[0];
            var debuggerLogoGuid = AssetDatabaseManager.FindAssets(
                ZString.Format("Debugger_Logo_{0}", EditorGUIUtility.isProSkin ? "White" : "Dark")
            )[0];
            var guiSkinGuid = AssetDatabaseManager.FindAssets("RuntimeGraphGUISkin")[0];

            _managerLogoTexture =
                AssetDatabaseManager.LoadAssetAtPath<Texture2D>(
                    AssetDatabaseManager.GUIDToAssetPath(managerLogoGuid)
                );

            _debuggerLogoTexture =
                AssetDatabaseManager.LoadAssetAtPath<Texture2D>(
                    AssetDatabaseManager.GUIDToAssetPath(debuggerLogoGuid)
                );

            m_skin = AssetDatabaseManager.LoadAssetAtPath<GUISkin>(
                AssetDatabaseManager.GUIDToAssetPath(guiSkinGuid)
            );

            if (m_skin != null)
            {
                m_headerStyle1 = m_skin.GetStyle("Header1");
                m_headerStyle2 = m_skin.GetStyle("Header2");

                SetGuiStyleFontColor(m_headerStyle2, EditorGUIUtility.isProSkin ? Color.white : Color.black);
            }
            else
            {
                m_headerStyle1 = new(boldLabelStyle);
                m_headerStyle2 = new(boldLabelStyle);
            }

            m_foldoutStyle = new GUIStyle(foldoutStyle);
            m_foldoutStyle.font = m_headerStyle2.font;
            m_foldoutStyle.fontStyle = m_headerStyle2.fontStyle;
            m_foldoutStyle.contentOffset = Vector2.down * 3f;

            SetGuiStyleFontColor(m_foldoutStyle, EditorGUIUtility.isProSkin ? Color.white : Color.black);
        }
    }
}

#endif
*/


