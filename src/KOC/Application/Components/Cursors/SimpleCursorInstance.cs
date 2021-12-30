using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Behaviours;
using Appalachia.Prototype.KOC.Application.Components.Cursors.Metadata;
using Appalachia.Utility.Async;
using Appalachia.Utility.Colors;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Components.Cursors
{
    public class SimpleCursorInstance : AppalachiaApplicationBehaviour<SimpleCursorInstance>
    {
        // [CallStaticConstructorInEditor] should be added to the class (initsingletonattribute)
        static SimpleCursorInstance()
        {
            RegisterDependency<CursorManager>(i => _cursorManager = i);
        }

        #region Static Fields and Autoproperties

        [FoldoutGroup("References")]
        [ShowInInspector]
        private static CursorManager _cursorManager;

        #endregion

        #region Fields and Autoproperties

        public SimpleCursorMetadata metadata;

        [SerializeField, HideLabel, BoxGroup("State Data"), InlineProperty]
        public CursorStateData stateData;

        [NonSerialized, ShowInInspector, ReadOnly]
        [PreviewField(64f, ObjectFieldAlignment.Right)]
        private Texture2D _texture;

        [NonSerialized, ShowInInspector, ReadOnly]
        private Rect _lastRect;

        #endregion

        #region Event Functions

        private void OnGUI()
        {
            using (_PRF_OnGUI.Auto())
            {
                if ((metadata == null) || (_texture == null))
                {
                    return;
                }

                Rect rect;

                if (stateData.locked)
                {
                    rect = _lastRect;
                }
                else
                {
                    var targetPosition = _cursorManager.currentPosition;

                    var screenSize = _cursorManager.screenSize;

                    var x = targetPosition.x;
                    var y = screenSize.y - targetPosition.y;

                    x -= metadata.hotspot.x;
                    y -= metadata.hotspot.y;

                    rect = new Rect(x, y, _texture.width, _texture.height);
                }

                _lastRect = rect;

                if (!stateData.visible)
                {
                    var realColor = stateData.color;

                    switch (stateData.state)
                    {
                        case CursorState.Normal:
                            break;
                        case CursorState.Hovering:
                            realColor *= Colors.Gray90;
                            break;
                        case CursorState.Pressed:
                            realColor *= Colors.Gray70;
                            break;
                        case CursorState.Disabled:
                            realColor *= Colors.Gray50;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    GUI.DrawTexture(
                        rect,
                        _texture,
                        ScaleMode.ScaleToFit,
                        true,
                        0f,
                        realColor,
                        Vector4.zero,
                        Vector4.zero
                    );
                }
            }
        }

        #endregion

        public void OnCursorColorChanged(Color color)
        {
            stateData.color = color;
        }

        public void OnCursorTypeChanged(CursorMetadata oldCursor, CursorMetadata newCursor)
        {
            using (_PRF_OnCursorTypeChanged.Auto())
            {
                if (newCursor is SimpleCursorMetadata nc)
                {
                    metadata = nc;
                    _texture = metadata.texture;
                    enabled = true;
                }
                else
                {
                    metadata = null;
                    _texture = null;
                    enabled = false;
                }
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);

                _cursorManager.CursorTypeChanged -= OnCursorTypeChanged;
                _cursorManager.CursorTypeChanged += OnCursorTypeChanged;
                _cursorManager.CursorColorChanged -= OnCursorColorChanged;
                _cursorManager.CursorColorChanged += OnCursorColorChanged;
                _cursorManager.CursorLockedChanged -= OnCursorLockedChanged;
                _cursorManager.CursorLockedChanged += OnCursorLockedChanged;
                _cursorManager.CursorStateChanged -= OnCursorStateChanged;
                _cursorManager.CursorStateChanged += OnCursorStateChanged;
                _cursorManager.CursorVisibilityChanged -= OnCursorVisibilityChanged;
                _cursorManager.CursorVisibilityChanged += OnCursorVisibilityChanged;
            }
        }

        private void OnCursorLockedChanged(bool locked)
        {
            stateData.locked = locked;
        }

        private void OnCursorStateChanged(CursorState state)
        {
            stateData.state = state;
        }

        private void OnCursorVisibilityChanged(bool visible)
        {
            stateData.visible = visible;
        }

        #region Profiling

        private const string _PRF_PFX = nameof(SimpleCursorInstance) + ".";

        private static readonly ProfilerMarker _PRF_OnGUI = new ProfilerMarker(_PRF_PFX + nameof(OnGUI));

        private static readonly ProfilerMarker _PRF_OnCursorTypeChanged =
            new ProfilerMarker(_PRF_PFX + nameof(OnCursorTypeChanged));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion

        /*private static Texture2D GetAdjustedCursorTexture(Texture2D texture, Color color)
        {
            using (_PRF_GetAdjustedCursorTexture.Auto())
            {
                var result = Instantiate(texture);

                result.Multiply(color);

                return result;
            }
        }*/
    }
}
