using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Behaviours;
using Appalachia.Prototype.KOC.Components.Cursors.Collections;
using Appalachia.Prototype.KOC.Components.Cursors.Metadata;
using Appalachia.Utility.Async;
using Appalachia.Utility.Enums;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Components.Cursors
{
    [ExecuteAlways]
    [ExecutionOrder(ExecutionOrders.CursorManager)]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Canvas))]
    [CallStaticConstructorInEditor]
    public class CursorManager : SingletonAppalachiaApplicationBehaviour<CursorManager>
    {
        public delegate void CursorColorChangeHandler(Color newColor);

        public delegate void CursorLockModeChangeHandler(bool locked);

        public delegate void CursorStateChangeHandler(CursorState state);

        public delegate void CursorTypeChangeHandler(CursorMetadata oldCursor, CursorMetadata newCursor);

        public delegate void CursorVisibilityChangeHandler(bool visible);

        public event CursorColorChangeHandler CursorColorChanged;
        public event CursorLockModeChangeHandler CursorLockedChanged;
        public event CursorStateChangeHandler CursorStateChanged;
        public event CursorTypeChangeHandler CursorTypeChanged;
        public event CursorVisibilityChangeHandler CursorVisibilityChanged;

        static CursorManager()
        {
            RegisterDependency<MainSimpleCursorLookup>(i => _mainSimpleCursorLookup = i);

            RegisterDependency<MainComplexCursorLookup>(i => _mainComplexCursorLookup = i);
        }

        #region Static Fields and Autoproperties

        [FoldoutGroup("Lookups")]
        [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        [ShowInInspector]
        private static MainComplexCursorLookup _mainComplexCursorLookup;

        [FoldoutGroup("Lookups")]
        [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        [ShowInInspector]
        private static MainSimpleCursorLookup _mainSimpleCursorLookup;

        #endregion

        #region Fields and Autoproperties

        [SerializeField, HideLabel, FoldoutGroup("State Data"), InlineProperty]
        public CursorStateData stateData;

        [BoxGroup("Current")]
        [NonSerialized, ShowInInspector, OnValueChanged(nameof(UIChangedCursorType))]
        private CursorMetadata _currentMetadata;

        [NonSerialized] private CursorMetadata _previousMetadata;

        [BoxGroup("Current")]
        [NonSerialized, ShowInInspector, OnValueChanged(nameof(UIChangedCursorColor))]
        private Color _currentColor;

        [FoldoutGroup("Lookups")]
        [NonSerialized, ShowInInspector, ReadOnly]
        private SimpleCursorInstance _simpleCursorInstance;

        [FoldoutGroup("Lookups")]
        [NonSerialized, ShowInInspector, ReadOnly]
        private ComplexCursorInstance _currentComplexCursorInstance;

        [FoldoutGroup("State Data")]
        [ReadOnly]
        private Vector2 _currentPosition;

        [FoldoutGroup("State Data")]
        [ReadOnly]
        private Vector2 _screenSize;

        [SerializeField]
        [ReadOnly, HideInInspector]
        private ComplexCursorInstanceLookup _complexCursorInstances;

        private RectTransform m_RectTransform;

        #endregion

        public Color CurrentColor => _currentColor;

        public CursorMetadata Current => _currentMetadata;

        /// <summary>
        ///     The RectTransform component used by the Graphic. Cached for speed.
        /// </summary>
        public RectTransform rectTransform
        {
            get
            {
                gameObject.GetOrCreateComponent(ref m_RectTransform);

                return m_RectTransform;
            }
        }

        public Vector2 currentPosition => _currentPosition;
        public Vector2 screenSize => _screenSize;

        #region Event Functions

        private void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (ShouldSkipUpdate)
                {
                    return;
                }

                var screenWidth = Screen.width;
                var screenHeight = Screen.height;

                _screenSize.x = screenWidth;
                _screenSize.y = screenHeight;

                if (AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    if (Mouse.current is { wasUpdatedThisFrame: true })
                    {
                        _currentPosition = Mouse.current.position.ReadValue();
                    }

                    if (Pointer.current is { wasUpdatedThisFrame: true })
                    {
                        _currentPosition = Pointer.current.position.ReadValue();
                    }

                    if (Gamepad.current is { wasUpdatedThisFrame: true })
                    {
                        _currentPosition += Gamepad.current.leftStick.ReadValue();
                        _currentPosition += Gamepad.current.rightStick.ReadValue();
                    }
                }
                else
                {
                    var updated = false;

                    if (Mouse.current != null)
                    {
                        updated = true;
                        var mousePosition = Mouse.current.position.ReadValue();

                        if (mousePosition == Vector2.zero)
                        {
                            _currentPosition = screenSize * .5f;
                        }
                        else
                        {
                            _currentPosition = mousePosition;
                        }
                    }

                    if (Gamepad.current != null)
                    {
                        _currentPosition += Gamepad.current.leftStick.ReadValue();
                        _currentPosition += Gamepad.current.rightStick.ReadValue();
                    }

                    if (!updated)
                    {
                        _currentPosition = screenSize * .5f;
                    }
                }

                currentPosition.ClampValue(Vector2.zero, new Vector2(screenWidth, screenHeight));
            }
        }

        #endregion

        public void ConfineSystemCursor()
        {
            using (_PRF_ConfineSystemCursor.Auto())
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
        }

        public void HideSystemCursor()
        {
            using (_PRF_HideSystemCursor.Auto())
            {
                Cursor.visible = false;
            }
        }

        public void LockSystemCursor()
        {
            using (_PRF_LockSystemCursor.Auto())
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        public void ReleaseSystemCursor()
        {
            using (_PRF_ReleaseSystemCursor.Auto())
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }

        public void SetCursorColor(Color color)
        {
            using (_PRF_SetCursorColor.Auto())
            {
                if (_currentColor != color)
                {
                    _currentColor = color;

                    CursorColorChanged?.Invoke(_currentColor);
                }
            }
        }

        public void SetCursorLocked(bool locked)
        {
            using (_PRF_SetCursorLocked.Auto())
            {
                CursorLockedChanged?.Invoke(locked);
            }
        }

        public void SetCursorState(CursorState state)
        {
            using (_PRF_SetCursorState.Auto())
            {
                CursorStateChanged?.Invoke(state);
            }
        }

        public void SetCursorType(ComplexCursors cursor)
        {
            using (_PRF_SetCursorType.Auto())
            {
                var targetMetadata = _mainComplexCursorLookup.Lookup[cursor];

                SetCursorTypeInternal(targetMetadata);
            }
        }

        public void SetCursorType(SimpleCursors cursor)
        {
            using (_PRF_SetCursorType.Auto())
            {
                var targetMetadata = _mainSimpleCursorLookup.Lookup[cursor];

                SetCursorTypeInternal(targetMetadata);
            }
        }

        public void SetCursorVisible(bool visible)
        {
            using (_PRF_SetCursorVisible.Auto())
            {
                CursorVisibilityChanged?.Invoke(visible);
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);
            using (_PRF_Initialize.Auto())
            {
#if UNITY_EDITOR
                if (_complexCursorInstances == null)
                {
                    _complexCursorInstances = new ComplexCursorInstanceLookup();
                }

                _complexCursorInstances.SetSerializationOwner(this);

                for (var i = 0; i < transform.childCount; i++)
                {
                    var child = transform.GetChild(i);

                    var childCursorInstance = child.GetComponent<ComplexCursorInstance>();

                    if (childCursorInstance == null)
                    {
                        continue;
                    }

                    var childType = childCursorInstance.metadata.complexCursorValue;

                    if (_complexCursorInstances.ContainsKey(childType))
                    {
                        _complexCursorInstances[childType] = childCursorInstance;
                    }
                    else
                    {
                        _complexCursorInstances.Add(childType, childCursorInstance);
                    }
                }

                foreach (var complexCursorType in EnumValueManager.GetAllValues<ComplexCursors>())
                {
                    if (!_complexCursorInstances.ContainsKey(complexCursorType))
                    {
                        if (!_mainComplexCursorLookup.Lookup.Items.ContainsKey(complexCursorType))
                        {
                            continue;
                        }

                        var complexCursorMetadata =
                            _mainComplexCursorLookup.Lookup.Items.Get(complexCursorType);

                        if (complexCursorMetadata == null)
                        {
                            continue;
                        }

                        if (complexCursorMetadata.prefab == null)
                        {
                            continue;
                        }

                        var prefabInstance = PrefabUtility.InstantiatePrefab(
                            complexCursorMetadata.prefab,
                            transform
                        ) as GameObject;

                        var cursorInstance = prefabInstance.GetComponent<ComplexCursorInstance>();

                        if (cursorInstance == null)
                        {
                            prefabInstance.DestroySafely();
                        }

                        _complexCursorInstances.Add(complexCursorType, cursorInstance);
                    }
                }
#endif
            }
        }

        [ButtonGroup("Confine")]
        private void ConfineSoftwareCursor()
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

        [ButtonGroup("Hide")]
        private void HideSoftwareCursor()
        {
            Cursor.visible = false;
        }

        [ButtonGroup("Lock")]
        private void LockSoftwareCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        [ButtonGroup("Confine")]
        private void ReleaseSoftwareCursor()
        {
            Cursor.lockState = CursorLockMode.None;
        }

        private void SetCursorTypeInternal(CursorMetadata targetMetadata)
        {
            using (_PRF_SetCursorTypeInternal.Auto())
            {
                if (targetMetadata != _currentMetadata)
                {
                    _previousMetadata = _currentMetadata;
                    _currentMetadata = targetMetadata;

                    CursorTypeChanged?.Invoke(_previousMetadata, _currentMetadata);

                    _previousMetadata = _currentMetadata;
                }
            }
        }

        [ButtonGroup("Hide")]
        private void ShowSoftwareCursor()
        {
            Cursor.visible = true;
        }

        private void UIChangedCursorColor()
        {
            CursorColorChanged?.Invoke(_currentColor);
        }

        private void UIChangedCursorType()
        {
            if (_currentMetadata is ComplexCursorMetadata cc)
            {
                if (_previousMetadata is ComplexCursorMetadata pc)
                {
                    CursorTypeChanged?.Invoke(pc, cc);
                }
                else if (_previousMetadata is SimpleCursorMetadata ps)
                {
                    CursorTypeChanged?.Invoke(ps, cc);
                }
                else
                {
                    CursorTypeChanged?.Invoke(null, cc);
                }
            }
            else if (_currentMetadata is SimpleCursorMetadata cs)
            {
                if (_previousMetadata is ComplexCursorMetadata pc)
                {
                    CursorTypeChanged?.Invoke(pc, cs);
                }
                else if (_previousMetadata is SimpleCursorMetadata ps)
                {
                    CursorTypeChanged?.Invoke(ps, cs);
                }
                else
                {
                    CursorTypeChanged?.Invoke(null, cs);
                }
            }

            _previousMetadata = _currentMetadata;
        }

        [ButtonGroup("Lock")]
        private void UnlockSoftwareCursor()
        {
            Cursor.lockState = CursorLockMode.None;
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_ConfineSystemCursor =
            new ProfilerMarker(_PRF_PFX + nameof(ConfineSystemCursor));

        private static readonly ProfilerMarker _PRF_HideSystemCursor =
            new ProfilerMarker(_PRF_PFX + nameof(HideSystemCursor));

        private static readonly ProfilerMarker _PRF_LockSystemCursor =
            new ProfilerMarker(_PRF_PFX + nameof(LockSystemCursor));

        private static readonly ProfilerMarker _PRF_ReleaseSystemCursor =
            new ProfilerMarker(_PRF_PFX + nameof(ReleaseSystemCursor));

        private static readonly ProfilerMarker _PRF_SetCursorColor =
            new ProfilerMarker(_PRF_PFX + nameof(SetCursorColor));

        private static readonly ProfilerMarker _PRF_SetCursorLocked =
            new ProfilerMarker(_PRF_PFX + nameof(SetCursorLocked));

        private static readonly ProfilerMarker _PRF_SetCursorState =
            new ProfilerMarker(_PRF_PFX + nameof(SetCursorState));

        private static readonly ProfilerMarker _PRF_SetCursorType =
            new ProfilerMarker(_PRF_PFX + nameof(SetCursorType));

        private static readonly ProfilerMarker _PRF_SetCursorTypeInternal =
            new ProfilerMarker(_PRF_PFX + nameof(SetCursorTypeInternal));

        private static readonly ProfilerMarker _PRF_SetCursorVisible =
            new ProfilerMarker(_PRF_PFX + nameof(SetCursorVisible));

        #endregion
    }
}
