using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Behaviours;
using Appalachia.Core.Extensions;
using Appalachia.Prototype.KOC.Application.Behaviours;
using Appalachia.Prototype.KOC.Application.Components.Cursors.Metadata;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Application.Components.Cursors
{
    [ExecuteAlways]
    [ExecutionOrder(ExecutionOrders.CursorManager)]
    public class CursorManager : SingletonAppalachiaApplicationBehaviour<CursorManager>
    {
        #region Fields and Autoproperties

        [NonSerialized, ShowInInspector, OnValueChanged(nameof(Reapply))]
        private bool _drawDirect;

        [NonSerialized, ShowInInspector, OnValueChanged(nameof(Reapply))]
        private CursorMetadata _currentMetadata;

        [NonSerialized, ShowInInspector, OnValueChanged(nameof(Reapply))]
        private Cursors _current;

        [NonSerialized, ShowInInspector, OnValueChanged(nameof(Reapply))]
        private Color _color;

        [NonSerialized, ShowInInspector, ReadOnly]
        [PreviewField(64f, ObjectFieldAlignment.Right)]
        private Texture2D _referenceTexture;

        [NonSerialized, ShowInInspector, ReadOnly]
        [PreviewField(64f, ObjectFieldAlignment.Right)]
        private Texture2D _texture;

        [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        public CursorMetadataLookup cursors;

        #endregion

        public Color Color => _color;

        public Cursors Current => _current;

        #region Event Functions

        [Button]
        private void ResetModifications()
        {
            SetCursor();
        }

        protected override void Start()
        {
            using (_PRF_Start.Auto())
            {
                Initialize();
            }
        }

        protected override void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                Initialize();
            }
        }

        private void OnGUI()
        {
            using (_PRF_Update.Auto())
            {
                if (UnityEngine.Application.isPlaying && !_drawDirect)
                {
                    return;
                }
                
                if (Mouse.current == null)
                {
                    return;
                }

                if (_texture == null)
                {
                    return;
                }

                var screenWidth = Screen.width;
                var screenHeight = Screen.height;

                var position = Mouse.current.position.ReadValue();

                var inScreen = (position.x >= 0) &&
                               (position.x < screenWidth) &&
                               (position.y >= 0) &&
                               (position.y < screenHeight);

                if (!inScreen)
                {
                    return;
                }

                var x = position.x;
                var y = screenHeight - position.y;

                x -= _currentMetadata.hotspot.x;
                y -= _currentMetadata.hotspot.y;

                var rect = new Rect(x, y, _texture.width, _texture.height);

                GUI.DrawTexture(
                    rect,
                    _texture,
                    ScaleMode.ScaleToFit,
                    true,
                    0f,
                    _color,
                    Vector4.zero,
                    Vector4.zero
                );
            }
        }

        #endregion

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();

                cursors = CursorMetadataLookup.instance;

                ResetCursor();
            }
        }

        public void ConfineCursor()
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

        public void HideCursor()
        {
            Cursor.visible = false;
        }

        public void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void ReleaseCursor()
        {
            Cursor.lockState = CursorLockMode.None;
        }

        public void ResetCursor()
        {
            ReleaseCursor();

            SetCursor();

            ShowCursor();
        }

        public void SetCursor(Cursors? cursor = null, Color? color = null)
        {
            if (cursor.HasValue)
            {
                _currentMetadata = cursors.Items[cursor.Value];
            }
            else
            {
                _currentMetadata = cursors.defaultValue;
            }

            if (cursor.HasValue)
            {
                _current = cursor.Value;
            }
            else
            {
                _current = cursors.defaultValue.value;
            }

            if (_color == default)
            {
                _color = Color.white;
            }

            if (color.HasValue)
            {
                _color = color.Value;
            }
            else
            {
                if (_currentMetadata.modifyColor)
                {
                    _color = _currentMetadata.cursorColor;
                }
            }

            if (color.HasValue)
            {
                ApplyCursor(_currentMetadata.texture, _color, _currentMetadata.hotspot);
            }
            else if (_currentMetadata.modifyColor)
            {
                ApplyCursor(_currentMetadata.texture, _currentMetadata.cursorColor, _currentMetadata.hotspot);
            }
            else
            {
                ApplyCursor(_currentMetadata.texture, _color, _currentMetadata.hotspot);
            }
        }

        public void SetLockState(CursorLockMode lockMode)
        {
            Cursor.lockState = lockMode;
        }

        public void ShowCursor()
        {
            Cursor.visible = true;
        }

        private void ApplyCursor(Texture2D texture, Color color, Vector2 hotspot)
        {
            if (_referenceTexture == null)
            {
                _referenceTexture = Instantiate(texture);
            }
            else if ((_referenceTexture.width == texture.width) &&
                     (_referenceTexture.height == texture.height))
            {
                _referenceTexture.Copy(texture);
            }
            else
            {
                _referenceTexture.DestroySafely();

                _referenceTexture = Instantiate(texture);
            }

            if (_texture == null)
            {
                _texture = Instantiate(_referenceTexture);
            }
            else if ((_texture.width == _referenceTexture.width) &&
                     (_texture.height == _referenceTexture.height))
            {
                _texture.Copy(_referenceTexture);
            }
            else
            {
                _texture.DestroySafely();

                _texture = Instantiate(_referenceTexture);
            }

            _texture.Multiply(color);

            _color = color;
            Cursor.SetCursor(_texture, hotspot, CursorMode.Auto);
        }

        private void Reapply()
        {
            using (_PRF_Reapply.Auto())
            {
                SetCursor(_current, _color);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(CursorManager) + ".";

        private static readonly ProfilerMarker _PRF_Update = new ProfilerMarker(_PRF_PFX + nameof(OnGUI));
        private static readonly ProfilerMarker _PRF_Reapply = new ProfilerMarker(_PRF_PFX + nameof(Reapply));

        private static readonly ProfilerMarker
            _PRF_OnEnable = new ProfilerMarker(_PRF_PFX + nameof(OnEnable));

        private static readonly ProfilerMarker _PRF_Start = new ProfilerMarker(_PRF_PFX + nameof(Start));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion
    }
}
