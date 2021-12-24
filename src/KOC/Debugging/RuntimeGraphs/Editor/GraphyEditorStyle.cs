#if UNITY_EDITOR
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Attributes;
using Appalachia.Utility.Strings;
using UnityEditor;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Editor
{
    [CallStaticConstructorInEditor]
    internal static class GraphyEditorStyle
    {
        #region Constants and Static Readonly

        private static readonly GUISkin m_skin;
        private static readonly GUIStyle m_foldoutStyle;
        private static readonly GUIStyle m_headerStyle1;
        private static readonly GUIStyle m_headerStyle2;
        private static readonly Texture2D _debuggerLogoTexture;

        private static readonly Texture2D _managerLogoTexture;

        #endregion

        #region Static Constructor

        static GraphyEditorStyle()
        {
            var managerLogoGuid = AssetDatabaseManager.FindAssets(
                ZString.Format("Manager_Logo_{0}", EditorGUIUtility.isProSkin ? "White" : "Dark")
            )[0];
            var debuggerLogoGuid = AssetDatabaseManager.FindAssets(
                ZString.Format("Debugger_Logo_{0}", EditorGUIUtility.isProSkin ? "White" : "Dark")
            )[0];
            var guiSkinGuid = AssetDatabaseManager.FindAssets("GraphyGUISkin")[0];

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
                m_headerStyle1 = new(EditorStyles.boldLabel);
                m_headerStyle2 = new(EditorStyles.boldLabel);
            }

            m_foldoutStyle = new GUIStyle(EditorStyles.foldout)
            {
                font = m_headerStyle2.font,
                fontStyle = m_headerStyle2.fontStyle,
                contentOffset = Vector2.down * 3f
            };

            SetGuiStyleFontColor(m_foldoutStyle, EditorGUIUtility.isProSkin ? Color.white : Color.black);
        }

        #endregion

        #region Static Fields and Autoproperties

        private static string path;

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
    }
}

#endif
