using Appalachia.Core.Scriptables;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Areas.Base
{
    [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
    public abstract class AreaMetadataBase<T> : SingletonAppalachiaObject<T>
        where T : AreaMetadataBase<T>
    {
        #region Profiling

        private const string _PRF_PFX = nameof(AreaMetadataBase<T>) + ".";

        private static readonly ProfilerMarker _PRF_Awake = new ProfilerMarker(_PRF_PFX + nameof(Awake));
        
        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        #endregion

        #region Fields

        [HideInInspector, SerializeField]
        private bool _initializedCanvasGroup;

        [HideInInspector, SerializeField]
        private bool _initializedCanvasScaling;

        [HideInInspector, SerializeField]
        private bool _initializedCanvas;

        [FoldoutGroup("Canvas Group")]
        public bool interactable;

        [FoldoutGroup("Canvas Group")]
        public bool blocksRaycasts;

        [FoldoutGroup("Canvas Group")]
        public bool ignoreParentGroups;

        [FoldoutGroup("Canvas Scaling")]
        public CanvasScaler.ScaleMode uiScaleMode;

        [FoldoutGroup("Canvas Scaling")]
        public Vector2 referenceResolution;

        [FoldoutGroup("Canvas Scaling")]
        public CanvasScaler.ScreenMatchMode screenMatchMode;

        [FoldoutGroup("Canvas Scaling")]
        [Range(0f, 1f)]
        public float match;

        [FoldoutGroup("Canvas Scaling")]
        [Range(1f, 200f)]
        public float referencePixelsPerUnit;

        [FoldoutGroup("Canvas")] public RenderMode renderMode;

        [FoldoutGroup("Canvas")] public bool pixelPerfect;

        [FoldoutGroup("Canvas")] public int sortingOrder;

        #endregion

        #region Event Functions

        private void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                if (!_initializedCanvasGroup)
                {
                    _initializedCanvasGroup = true;

                    interactable = true;
                    blocksRaycasts = true;
                    ignoreParentGroups = false;
                }

                if (!_initializedCanvasScaling)
                {
                    _initializedCanvasScaling = true;

                    uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    referenceResolution = new Vector2(1920f, 1080f);
                    screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                    match = 1f;
                    referencePixelsPerUnit = 100f;
                }

                if (!_initializedCanvas)
                {
                    renderMode = RenderMode.ScreenSpaceOverlay;
                    pixelPerfect = false;
                    sortingOrder = 0;
                }
            }
        }

        #endregion

        public void Apply(Canvas c)
        {
            using (_PRF_Apply.Auto())
            {
                c.renderMode = renderMode;
                c.pixelPerfect = pixelPerfect;
                c.sortingOrder = sortingOrder;
            }
        }

        public void Apply(CanvasScaler cs)
        {
            using (_PRF_Apply.Auto())
            {
                cs.uiScaleMode = uiScaleMode;
                cs.referenceResolution = referenceResolution;
                cs.screenMatchMode = screenMatchMode;
                cs.matchWidthOrHeight = match;
                cs.referencePixelsPerUnit = referencePixelsPerUnit;
            }
        }

        public void Apply(CanvasGroup cg)
        {
            using (_PRF_Apply.Auto())
            {
                cg.interactable = interactable;
                cg.blocksRaycasts = blocksRaycasts;
                cg.ignoreParentGroups = ignoreParentGroups;
            }
        }
    }
}
