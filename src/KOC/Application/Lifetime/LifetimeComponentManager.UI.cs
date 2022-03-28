using Appalachia.CI.Constants;
using Appalachia.UI.Functionality.Canvas.Controls.Root;
using Appalachia.UI.Functionality.Images.Controls.Background;
using Appalachia.Utility.Events;
using Appalachia.Utility.Events.Extensions;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Lifetime
{
    public partial class LifetimeComponentManager
    {
        #region Constants and Static Readonly

        private const string GROUP_UI = GROUP_BASE + PARENT_NAME_UI;

        private const string PARENT_NAME_UI = "UI";

        #endregion

        #region Fields and Autoproperties

        [FoldoutGroup(GROUP_UI)]
        [SerializeField]
        private RootCanvasControl _rootCanvas;

        [FoldoutGroup(GROUP_UI)]
        [SerializeField]
        private BackgroundControl _rootBackground;

        [FoldoutGroup(GROUP_UI), SerializeField]
        private GameObject _uiObject;

        public AppaEvent<RootCanvasControl>.Data RootCanvasControlReady;

        #endregion

        public GameObject UIObject => _uiObject;

        public Rect ScaledCanvasBounds => ScaledCanvas.rect;

        public RectTransform ScaledCanvas => _rootCanvas.ScaledCanvas.transform as RectTransform;
        public RootCanvasControl RootCanvasControl => _rootCanvas;

        public Vector2 GetPositionInScaledCanvas(Vector2 position)
        {
            using (_PRF_GetPositionInScaledCanvas.Auto())
            {
                var isInBounds = ScreenPointToScaledCanvasPoint(position, out var localPosition);

                return localPosition;
            }
        }

        public bool ScreenPointToLocalPointInRectangle(
            RectTransform rect,
            Vector2 screenPoint,
            Camera cam,
            out Vector2 localPoint)
        {
            using (_PRF_ScreenPointToLocalPointInRectangle.Auto())
            {
                localPoint = Vector2.zero;
                Vector3 worldPoint;
                if (!ScreenPointToWorldPointInRectangle(rect, screenPoint, cam, out worldPoint))
                {
                    return false;
                }

                localPoint = rect.InverseTransformPoint(worldPoint);
                return true;
            }
        }

        public Ray ScreenPointToRay(Camera cam, Vector2 screenPos)
        {
            using (_PRF_ScreenPointToRay.Auto())
            {
                if (cam != null)
                {
                    return cam.ScreenPointToRay(screenPos);
                }

                Vector3 origin = screenPos;
                origin.z -= 100f;
                return new Ray(origin, Vector3.forward);
            }
        }

        public bool ScreenPointToScaledCanvasPoint(Vector2 screenPoint, out Vector2 localPoint)
        {
            using (_PRF_ScreenPointToScaledCanvasPoint.Auto())
            {
                return ScreenPointToLocalPointInRectangle(ScaledCanvas, screenPoint, null, out localPoint);
            }
        }

        public bool ScreenPointToWorldPointInRectangle(
            RectTransform rect,
            Vector2 screenPoint,
            Camera cam,
            out Vector3 worldPoint)
        {
            using (_PRF_ScreenPointToWorldPointInRectangle.Auto())
            {
                worldPoint = Vector2.zero;
                var ray = ScreenPointToRay(cam, screenPoint);
                var plane = new Plane(rect.rotation * Vector3.back, rect.position);
                var enter = 0.0f;
                if ((Vector3.Dot(Vector3.Normalize(rect.position - ray.origin), plane.normal) != 0.0) &&
                    !plane.Raycast(ray, out enter))
                {
                    return false;
                }

                worldPoint = ray.GetPoint(enter);
                return true;
            }
        }

        private void InitializeUI()
        {
            using (_PRF_InitializeUI.Auto())
            {
                gameObject.GetOrAddChild(ref _uiObject, PARENT_NAME_UI, false);

                RootCanvasControl.Refresh(ref _rootCanvas, _uiObject, nameof(_rootCanvas));
                _lifetimeMetadata.rootCanvas.Apply(_rootCanvas);

                BackgroundControl.Refresh(ref _rootBackground, _rootCanvas.ScaledCanvas.gameObject, nameof(_rootCanvas));
                _lifetimeMetadata.rootBackground.Apply(_rootBackground);

                RootCanvasControlReady.RaiseEvent(RootCanvasControl);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetPositionInScaledCanvas =
            new ProfilerMarker(_PRF_PFX + nameof(GetPositionInScaledCanvas));

        private static readonly ProfilerMarker _PRF_InitializeUI = new ProfilerMarker(_PRF_PFX + nameof(InitializeUI));

        private static readonly ProfilerMarker _PRF_ScreenPointToLocalPointInRectangle =
            new ProfilerMarker(_PRF_PFX + nameof(ScreenPointToLocalPointInRectangle));

        private static readonly ProfilerMarker _PRF_ScreenPointToRay =
            new ProfilerMarker(_PRF_PFX + nameof(ScreenPointToRay));

        private static readonly ProfilerMarker _PRF_ScreenPointToScaledCanvasPoint =
            new ProfilerMarker(_PRF_PFX + nameof(ScreenPointToScaledCanvasPoint));

        private static readonly ProfilerMarker _PRF_ScreenPointToWorldPointInRectangle =
            new ProfilerMarker(_PRF_PFX + nameof(ScreenPointToWorldPointInRectangle));

        #endregion
    }
}
